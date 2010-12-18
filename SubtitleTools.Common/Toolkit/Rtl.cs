﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTools.Common.Toolkit
{
    public static class Rtl
    {
        public static string ApplyUnifiedYeKe(this string data)
        {
            if (string.IsNullOrEmpty(data)) return data;
            return data.Replace("ي", "ی").Replace("ك", "ک");
        }
    }
}
