using System;
using System.Windows;
using System.Windows.Threading;

namespace SubtitleTools.Common.Threading
{
    public class DispatcherHelper
    {
        public static void DispatchAction(Action func)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, func);
        }
    }
}
