using System;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Common.Logger;
using SubtitleTools.Common.Files;
using System.Reflection;

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

            this.Startup += appStartup;
            this.Exit += appExit;
            this.DispatcherUnhandledException += appDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += currentDomainUnhandledException;
            createFileAssociation();
        }

        #endregion Constructors

        #region Methods (5)

        // Private Methods (5) 

        private static void appDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionLogger.LogExceptionToFile(e.Exception);
            LogWindow.AddMessage(LogType.Error, e.Exception.Message);
            e.Handled = true;
        }

        static void appExit(object sender, ExitEventArgs e)
        {
        }

        void appStartup(object sender, StartupEventArgs e)
        {
		    ReducingCpuConsumptionForAnimations();
            if (e.Args.Any())
            {
                this.Properties["StartupFileName"] = e.Args[0];
            }
        }
		
		void ReducingCpuConsumptionForAnimations()
		{
           Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 20 }
                );
		}

        private static void createFileAssociation()
        {
            var appPath = Assembly.GetExecutingAssembly().Location;
            FileAssociation.CreateFileAssociation(".srt", "Sub", "Subtitle File",
                appPath
                );
        }

        static void currentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            ExceptionLogger.LogExceptionToFile(ex);
        }

        #endregion Methods
    }
}
