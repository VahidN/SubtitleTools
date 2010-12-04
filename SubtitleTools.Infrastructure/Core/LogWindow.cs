using System;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.Core
{
    public enum LogType
    {
        Alert,
        Error,
        Info,
        Announcement
    }

    public class LogWindow
    {
        public static void AddMessage(LogType type, string msg)
        {           
            App.Messenger.NotifyColleagues("AddLog",
                new Log
                {
                    Message = msg,
                    Type = type.ToString(),
                    Time = DateTime.Now
                }
                );
        }
    }
}
