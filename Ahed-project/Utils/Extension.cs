using Ahed_project.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Utils
{
    public static class Extension
    {
        public static string ToNormalCase(this string s)
        {
            if (s == null || s.Length == 0)
                return s;
            var processing = new string(s);
            processing = processing.Replace("_", " ");
            var first = processing.Substring(0, 1).ToUpper();
            var other = processing.Substring(1, processing.Length - 1);
            processing = first + other;
            return processing;
        }

        public static string ToDoubleWithoutDot(this string s)
        {
            if (s=="NaN")
            {
                return s;
            }
            if (s == null || s.Length == 0)
                return s;
            var processing = new string(s);
            var d = StringToDoubleChecker.ConvertToDouble(processing);
            return d.ToString("F");
        }
    }
}
