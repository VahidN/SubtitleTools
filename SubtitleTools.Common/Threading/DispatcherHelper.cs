using System;
using System.Windows;
using System.Windows.Threading;

namespace SubtitleTools.Common.Threading
{
    public class DispatcherHelper
    {
        public static void DispatchAction(Action func)
        {
            Dispatcher dispatcher;
            if (Application.Current != null)
            {
                dispatcher = Application.Current.Dispatcher;
            }
            else
            {
                //this is useful for unit tests where there is no application running 
                dispatcher = Dispatcher.CurrentDispatcher;
            }
			
			if (func == null || dispatcher == null)
                return;

            dispatcher.Invoke(DispatcherPriority.ApplicationIdle, func);
		}
    }
}
