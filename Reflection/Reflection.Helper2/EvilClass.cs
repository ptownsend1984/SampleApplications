using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reflection.Helper2
{
    public class EvilClass
    {

        #region Methods

        public void ClearMainWindow()
        {
            var applicationType = Type.GetType("System.Windows.Application, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            var currentAppProperty = applicationType.GetProperty("Current", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            var currentApp = currentAppProperty.GetValue(null);

            var mainWindowProperty = applicationType.GetProperty("MainWindow");
            var mainWindow = mainWindowProperty.GetValue(currentApp, null);

            var mainWindowType = Type.GetType("Reflection.MainWindow, Reflection");
            var clearViewMethod = mainWindowType.GetMethod("ClearMainView", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            clearViewMethod.Invoke(mainWindow, new object[] { });
        }

        #endregion

    }
}