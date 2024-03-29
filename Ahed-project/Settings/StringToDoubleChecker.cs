﻿using Ahed_project.MasterData;
using System;
using System.Globalization;
using System.Linq;

namespace Ahed_project.Settings
{
    public class StringToDoubleChecker
    {
        private static readonly string curr = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string inv = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
        public static string ConvertFromInvariantCulture(string s)
        {
            return s?.Replace(inv, curr);
        }

        public static double ConvertToDouble(string s)
        {
            if (String.IsNullOrEmpty(s)) return 0;
            if (!Boolean.TryParse(s, out _))
            {
                return Convert.ToDouble(ConvertFromInvariantCulture(RemoveLetters(s, out var q)));
            }
            return 0;
        }

        public static string RemoveLetters(string s, out bool changedCount)
        {
            changedCount = false;
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }
            int n = s.Length;
            if (s.First() == '.' || s.First() == ',')
            {
                s = s.Remove(0, 1);
            }
            s = new string((from c in s
                            where char.IsDigit(c) || c == '.' || c == ','
                            select c).ToArray());
            changedCount = n != s.Length;
            return ToCorrectFormat(s);
        }

        public static string ToCorrectFormat(string s)
        {
            if (s == null)
            {
                return s;
            }
            s = s.Replace('.', Config.DoubleSplitter).Replace(',', Config.DoubleSplitter);
            if (s.Where(x => x == Config.DoubleSplitter).Count() > 1)
            {
                var ind = s.IndexOf(Config.DoubleSplitter);
                for (int i = ind + 1; i < s.Length; i++)
                {
                    if (s[i] == Config.DoubleSplitter)
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
