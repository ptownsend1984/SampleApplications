using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading;

namespace FolderCrawlerDemo.Controllers
{
    public class MainWindowController : UINotificationObject, IDisposable
    {

        #region Constants

        private const int SEMAPHORE_COUNT = 5;
        private int FOLDER_CRAWL_DELAY = 50;
        private int FILE_CRAWL_DELAY = 50;

        #endregion

        #region Global Variables

        private readonly Semaphore CrawlerSemaphore;

        private string _SelectedFolderPath;
        private readonly DelegateCommand _ChooseFolderCommand;

        private int _IsRunning;
        private bool _IsCanceled;
        private readonly DelegateCommand _StartCommand;
        private readonly DelegateCommand _StopCommand;

        private readonly object MessageLock = new object();
        private string _MessageText;

        #endregion

        #region Properties

        public string SelectedFolderPath
        {
            get { return _SelectedFolderPath; }
            set
            {
                if (_SelectedFolderPath == value) return;
                _SelectedFolderPath = value;
                OnPropertyChanged(() => this.SelectedFolderPath);
                this.RaiseCrawlCommands();
            }
        }
        public ICommand ChooseFolderCommand { get { return _ChooseFolderCommand; } }

        public bool IsRunning
        {
            get { return _IsRunning > 0; }
            set
            {
                if (value)
                    Interlocked.Increment(ref _IsRunning);
                else
                    Interlocked.Decrement(ref _IsRunning);
                OnPropertyChanged(() => this.IsRunning);
                this.RaiseChooseFolderCommand();
                this.RaiseCrawlCommands();
            }
        }
        public bool IsCanceled
        {
            get { return _IsCanceled; }
            set
            {
                if (_IsCanceled == value) return;
                _IsCanceled = value;
                OnPropertyChanged(() => this.IsCanceled);
                this.RaiseStopCommand();
            }
        }
        public ICommand StartCommand { get { return _StartCommand; } }
        public ICommand StopCommand { get { return _StopCommand; } }

        public string MessageText
        {
            get { return _MessageText; }
            private set
            {
                if (_MessageText == value) return;
                _MessageText = value;
                OnPropertyChanged(() => this.MessageText);
            }
        }

        public override string FormText
        {
            get
            {
                return "Folder Crawler Demo";
            }
            set
            {
                base.FormText = value;
            }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public MainWindowController()
        {
            _ChooseFolderCommand = new DelegateCommand(DoChooseFolderCommand, CanDoChooseFolderCommand);
            _StartCommand = new DelegateCommand(DoStartCommand, CanDoStartCommand);
            _StopCommand = new DelegateCommand(DoStopCommand, CanDoStopCommand);

            //Use the SEMAPHORE_COUNT constant to control the maximum number of concurrent directory crawlers
            CrawlerSemaphore = new Semaphore(SEMAPHORE_COUNT, SEMAPHORE_COUNT);
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        private void DoChooseFolderCommand()
        {
            if (!this.CanDoChooseFolderCommand())
                return;

            var Dialog = new System.Windows.Forms.FolderBrowserDialog();
            Dialog.SelectedPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Dialog.Description = "Choose folder path";
            var Result = Dialog.ShowDialog(this.Owner as System.Windows.Forms.IWin32Window);
            if (Result != System.Windows.Forms.DialogResult.OK)
                return;

            this.SelectedFolderPath = Dialog.SelectedPath;
        }
        private bool CanDoChooseFolderCommand()
        {
            return !this.IsRunning;
        }

        private void DoStartCommand()
        {
            if (!this.CanDoStartCommand())
                return;

            this.IsRunning = true;
            this.IsCanceled = false;
            this.ClearMessage();
            var BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            BackgroundWorker.DoWork += (s, e) =>
                {
                    this.UseWaitCursor = true;
                    try
                    {
                        var SelectedFolderPath = this.SelectedFolderPath;
                        if (string.IsNullOrEmpty(SelectedFolderPath) || !System.IO.Directory.Exists(SelectedFolderPath))
                            return;

                        var RootDirectoryInfo = new System.IO.DirectoryInfo(SelectedFolderPath);
                        CrawlRootFolder(RootDirectoryInfo);
                    }
                    finally
                    {
                        this.UseWaitCursor = false;
                        this.IsRunning = false;
                    }
                };
            BackgroundWorker.RunWorkerCompleted += (s, e) =>
                {
                    if (e.Error != null)
                        this.ShowErrorMessageBox(e.Error);
                    else if (!this.IsCanceled)
                        this.AppendMessage("Crawling completed.");
                    else
                        this.AppendMessage("Crawling canceled.");
                };
            BackgroundWorker.RunWorkerAsync();
        }
        private void CrawlRootFolder(System.IO.DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException("directoryInfo");

            if (this.IsCanceled)
                return;
            
            //First crawl the root folder's files
            this.CrawlFiles(directoryInfo);
            if (this.IsCanceled)
                return;

            //Use a ManualResetEvent to keep this function from ending before all the
            //crawlers finish.
            using (var ResetEvent = new ManualResetEvent(false))
            {
                var Directories = directoryInfo.GetDirectories();
                int CompletedCount = 0;
                for (int i = 0; i < Directories.Length; i++)
                {
                    var Item = Directories[i];
                    bool IsLast = i == Directories.Length - 1;

                    //Using another background worker could get bad since they use the threadpool.
                    //This could probably be better optimized to avoid that problem.
                    var Thread = new System.Threading.Thread((ThreadStart)(delegate 
                        {
                            this.CrawlerSemaphore.WaitOne();
                            try
                            {
                                RecurseCrawlFolder(Item);
                            }
                            catch(Exception ex)
                            {
                                this.AppendMessage(ex.Message);
                                return;
                            }
                            finally
                            {
                                Interlocked.Increment(ref CompletedCount);
                                this.CrawlerSemaphore.Release();
                                if (CompletedCount == Directories.Length)
                                    ResetEvent.Set();
                            }
                        }));
                    Thread.IsBackground = true;
                    Thread.Start();
                }

                ResetEvent.WaitOne();
            }
        }
        private void RecurseCrawlFolder(System.IO.DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException("directoryInfo");

            if (this.IsCanceled)
                return;

            //Recursively loop through subsequent files and folders
            var Directories = CrawlFolder(directoryInfo);
            if (this.IsCanceled)
                return;

            foreach (var Item in Directories)
            {
                if (this.IsCanceled)
                    return;

                RecurseCrawlFolder(Item);
            }
        }
        private System.IO.DirectoryInfo[] CrawlFolder(System.IO.DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException("directoryInfo");

            if (this.IsCanceled)
                return null;

            this.AppendMessage(string.Format("Crawling directory '{0}'.", directoryInfo.FullName));

            if (FOLDER_CRAWL_DELAY > 0)
                System.Threading.Thread.Sleep(FOLDER_CRAWL_DELAY);

            CrawlFiles(directoryInfo);
            if (!this.IsCanceled)
                return directoryInfo.GetDirectories();
            else
                return null;
        }
        private void CrawlFiles(System.IO.DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException("directoryInfo");

            if (this.IsCanceled)
                return;

            foreach (var Item in directoryInfo.GetFiles())
            {
                if (this.IsCanceled)
                    return;

                if (FILE_CRAWL_DELAY > 0)
                    System.Threading.Thread.Sleep(FILE_CRAWL_DELAY);
                this.AppendMessage(string.Format("Found file '{0}'", Item.FullName));
            }
        }
        private bool CanDoStartCommand()
        {
            return !this.IsRunning && !string.IsNullOrEmpty(this.SelectedFolderPath);
        }

        private void DoStopCommand()
        {
            if (!CanDoStopCommand())
                return;

            this.IsCanceled = true;
        }
        private bool CanDoStopCommand()
        {
            return this.IsRunning && !this.IsCanceled;
        }

        private void AppendMessage(string message)
        {            
            //Lock the message text when updating.
            //The WPF text box seems to flip out if you don't limit the max length of the message.
            //4000 worked pretty good.
            lock (this.MessageLock)
            {
                var Message = new System.Text.StringBuilder(this.MessageText);
                Message.AppendLine();
                Message.Append(message);
                this.MessageText = Message.Length <= 4000 ? Message.ToString() : Message.ToString().Substring(Message.Length - 4000);
            }
        }
        private void ClearMessage()
        {
            //Lock the messages when clearing
            lock (this.MessageLock)
            {
                this.MessageText = string.Empty;
            }
        }

        private void RaiseChooseFolderCommand()
        {
            this.Invoke(new Action(this._ChooseFolderCommand.RaiseCanExecuteChanged));
        }
        private void RaiseCrawlCommands()
        {
            this.RaiseStartCommand();
            this.RaiseStopCommand();
        }
        private void RaiseStartCommand()
        {
            this.Invoke(new Action(this._StartCommand.RaiseCanExecuteChanged));
        }
        private void RaiseStopCommand()
        {
            this.Invoke(new Action(this._StopCommand.RaiseCanExecuteChanged));
        }

        #endregion


        public void Dispose()
        {
            ((IDisposable)this.CrawlerSemaphore).Dispose();
        }
    }
}