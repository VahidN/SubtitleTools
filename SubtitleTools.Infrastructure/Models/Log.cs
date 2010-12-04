using System;

namespace SubtitleTools.Infrastructure.Models
{
    public class Log
    {
        public string Type { set; get; }
        public DateTime Time { set; get; }        
        public string Message { set; get; }
    }
}
