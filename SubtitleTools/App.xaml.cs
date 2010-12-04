using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Common.Logger;

namespace SubtitleTools
{
    public partial class App
    {
        #region Constructors (1)

        public App()
        {
            //WPF Single Instance Application
            var process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (process.Length > 1)
            {
                MessageBox.Show(
                    "SubtitleTools is already running ...",
                    "SubtitleTools",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Shutdown();
            }

            this.Exit += appExit;
            this.DispatcherUnhandledException += appDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += currentDomainUnhandledException;
        }

        #endregion Constructors

        #region Methods (3)

        // Private Methods (3) 

        private static void appDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionLogger.LogExceptionToFile(e.Exception);
            LogWindow.AddMessage(LogType.Error, e.Exception.Message);
            e.Handled = true;
        }

        static void appExit(object sender, ExitEventArgs e)
        {
        }

        static void currentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            ExceptionLogger.LogExceptionToFile(ex);
        }

        #endregion Methods
    }
}
