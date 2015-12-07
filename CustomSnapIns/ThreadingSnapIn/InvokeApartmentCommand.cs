using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Threading;
using System.Management.Automation.Runspaces;

namespace CustomSnapIns
{
    
    [Cmdlet("Invoke", "Apartment")]
    public class InvokeApartmentCommand : PSCmdlet
    {

        #region Global Variables

        private Runspace Runspace;

        #endregion

        #region Properties

        [Parameter(Position = 0, Mandatory = true)]
        public ApartmentState Apartment { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
        public string Expression { get; set; }

        #endregion

        #region Methods

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            this.Runspace = Runspace.DefaultRunspace;
        }
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            ExecutionResult result;
            if (Thread.CurrentThread.GetApartmentState() == this.Apartment)
            {
                result = ExecuteExpression(this.Expression);
            }
            else
            {
                var ApartmentThread = new System.Threading.Thread(StartExecuteExpression);
                ApartmentThread.SetApartmentState(this.Apartment);

                var Parameters = new ExecutionParameters { Expression = this.Expression };
                ApartmentThread.Start(Parameters);
                ApartmentThread.Join();

                result = Parameters.Result;
            }
            if (result == null)
                throw new InvalidOperationException("No result returned.");
            if (result.Error != null)
                throw result.Error;
            if (result.Output != null)
                WriteObject(result.Output);
        }

        private void StartExecuteExpression(object state)
        {
            if (state == null)
                throw new ArgumentNullException("state");

            var Parameters = state as ExecutionParameters;
            if (Parameters == null)
                throw new InvalidCastException("Cannot cast state to ExecutionParameters");

            Runspace.DefaultRunspace = this.Runspace;
            Parameters.Result = ExecuteExpression(Parameters.Expression);
        }
        private ExecutionResult ExecuteExpression(string expression)
        {
            try
            {
                var ScriptBlock = InvokeCommand.NewScriptBlock(this.Expression);
                return new ExecutionResult { Output = ScriptBlock.InvokeReturnAsIs(null) };
            }
            catch (Exception ex)
            {
                return new ExecutionResult { Error = ex };
            }
        }

        #endregion

    }

    internal class ExecutionParameters
    {
        public string Expression { get; set; }
        public ExecutionResult Result { get; set; }
    }
    internal class ExecutionResult
    {
        public object Output { get; set; }
        public Exception Error { get; set; }
    }
}