using System;
using System.Diagnostics;

namespace SubtitleTools.Common.Toolkit
{
    public static class Memory
    {
        public static void ReEvaluatedWorkingSet()
        {
            try
            {
                Process loProcess = Process.GetCurrentProcess();
                //it doesn't matter what you set maxWorkingSet to
                //setting it to any value apparently causes the working set to be re-evaluated and excess discarded
                loProcess.MaxWorkingSet = (IntPtr)((int)loProcess.MaxWorkingSet + 1);
            }
            catch
            {
                //The above code requires Admin privileges. 
                //So it's important to trap exceptions in case you're running without admin rights. 
            }
        }
    }
}
