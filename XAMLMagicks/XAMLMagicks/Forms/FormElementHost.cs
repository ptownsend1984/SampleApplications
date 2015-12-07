using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Reflection;
using System.Windows.Forms.Integration;

namespace XAMLMagicks.Forms
{
    public class FormElementHost : ElementHost
    {

        #region Static Members

        private readonly static KeyboardNavigation KeyboardNavigation;
        private readonly static MethodInfo _GetNextTab;
        private readonly static MethodInfo _GetPrevTab;
        private readonly static MethodInfo _GetGroupParent;

        static FormElementHost()
        {
            //Reflect out the KeyboardNavigation singleton and some methods
            KeyboardNavigation = (KeyboardNavigation)typeof(System.Windows.FrameworkElement).GetProperty("KeyboardNavigation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null, null);
            _GetNextTab = KeyboardNavigation.GetType().GetMethod("GetNextTab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            _GetPrevTab = KeyboardNavigation.GetType().GetMethod("GetPrevTab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            _GetGroupParent = KeyboardNavigation.GetType().GetMethod("GetGroupParent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new Type[] { typeof(System.Windows.DependencyObject), typeof(bool) }, null);
        }
        private static DependencyObject GetNextTab(DependencyObject e, DependencyObject container, bool goDownOnly)
        {
            var Method = _GetNextTab;
            if (Method == null)
                return null;
            return Method.Invoke(KeyboardNavigation, new object[] { e, container, goDownOnly }) as DependencyObject;
        }
        private static DependencyObject GetPrevTab(DependencyObject e, DependencyObject container, bool goDownOnly)
        {
            var Method = _GetPrevTab;
            if (Method == null)
                return null;
            return Method.Invoke(KeyboardNavigation, new object[] { e, container, goDownOnly }) as DependencyObject;
        }
        private static DependencyObject GetGroupParent(DependencyObject e, bool includeCurrent)
        {
            var Method = _GetGroupParent;
            if (Method == null)
                return null;
            return Method.Invoke(KeyboardNavigation, new object[] { e, includeCurrent }) as DependencyObject;
        }

        #endregion

        #region Properties

        protected HwndSource HwndSourceInternal { get { return (HwndSource)this.GetType().GetProperty("HwndSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this, null); } }
        protected IKeyboardInputSink Sink { get { return (IKeyboardInputSink)HwndSourceInternal; } }

        #endregion

        #region Methods

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            //The problem addressed here had to do with tab wonkiness with MDI forms.
            //When you tabbed forward through the inner WPF control to the end, the tab focus would jump out of the 
            //form and into the next control or MDI child window under the MDI form.
            //As best I could tell, the tab logic was being confused by MDI.
            //The solution used here is to reflect out methods on the KeyboardNavigation class
            //to determine what the next/previous item would be in the tab order.
            //(The exact parameters were gleaned by disassembling the private
            //bool Navigate(DependencyObject currentElement, TraversalRequest request, ModifierKeys modifierKeys, DependencyObject firstElement)
            //method on the KeyboardNavigation class.
            //If the destination element is null, then the end of the tab order had been reached.
            //Instead of relying on the base ProcessCmdKey, specifically tell the IKeyboardInputSink HwndSource
            //to move to the beginning or end of the embedded WPF tab order.
            if (keyData == Keys.Tab)
            {
                var FocusedElement = Keyboard.FocusedElement as UIElement;
                if (FocusedElement != null)
                {
                    DependencyObject DestinationElement = null;
                    FocusNavigationDirection Direction;
                    if (keyData.HasFlag(Keys.Shift))
                    {
                        Direction = FocusNavigationDirection.Last;
                        DestinationElement = GetPrevTab(FocusedElement, null, false);
                    }
                    else
                    {
                        Direction = FocusNavigationDirection.First;
                        DestinationElement = GetNextTab(FocusedElement, GetGroupParent(FocusedElement, true), false);
                    }
                    if (DestinationElement == null)
                        return this.Sink.TabInto(new TraversalRequest(Direction));
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override bool ProcessDialogKey(System.Windows.Forms.Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                var FocusedElement = Keyboard.FocusedElement as UIElement;
                if (FocusedElement != null)
                {
                    DependencyObject DestinationElement = null;
                    FocusNavigationDirection Direction;
                    if (keyData.HasFlag(Keys.Shift))
                    {
                        Direction = FocusNavigationDirection.Last;
                        DestinationElement = GetPrevTab(FocusedElement, null, false);
                    }
                    else
                    {
                        Direction = FocusNavigationDirection.First;
                        DestinationElement = GetNextTab(FocusedElement, GetGroupParent(FocusedElement, true), false);
                    }
                    if (DestinationElement == null)
                    {
                        return this.Sink.TabInto(new TraversalRequest(Direction));
                    }
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        #endregion

    }
}