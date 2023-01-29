using Ahed_project.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Settings
{
    public class StringToDoubleChecker
    {
        public static double ConvertToDouble(string s)
        {
            return Convert.ToDouble(s.Replace('.', ','));
        }

        public static string RemoveLetters(string s, out bool changedCount)
        {
            changedCount = false;
            int n = s.Length;
            s = new string((from c in s
                           where char.IsDigit(c) || c == '.' || c == ','
                           select c).ToArray());
            changedCount = n != s.Length;
            return ToCorrectFormat(s);
        }

        public static string ToCorrectFormat(string s)
        {            
            s= s.Replace( '.', Config.DoubleSplitter).Replace(',',Config.DoubleSplitter);
            if (s.Where(x => x == Config.DoubleSplitter).Count()>1)
            {
                var ind = s.IndexOf(Config.DoubleSplitter);
                for (int i = ind+1;i<s.Length;i++)
                {
                    if (s[i]== Config.DoubleSplitter)
                    {
                        s = s.Remove(i, 1);
                        i--;
                    }
                }
            }
            return s;
        }
    }
}
