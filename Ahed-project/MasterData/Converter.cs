using Ahed_project.Settings;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ahed_project.MasterData
{
    public class Converter : IMultiValueConverter, IValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return null;
            }
            if (Config.NumberOfDecimals == 0)
            {
                return values[0]?.ToString();
            }
            var doubleValue = System.Convert.ToDouble(StringToDoubleChecker.ConvertFromInvariantCulture(values[0]?.ToString()));
            if (doubleValue < 0.1 * Math.Pow(10,Config.NumberOfDecimals - 1))
            {
                return StringToDoubleChecker.ToCorrectFormat(doubleValue.ToString());
            }
            else 
            {
                string value = StringToDoubleChecker.ToCorrectFormat(doubleValue.ToString($"F{Config.NumberOfDecimals}"));
                return value;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == DependencyProperty.UnsetValue)
                {
                    return null;
                }
                if (Config.NumberOfDecimals == 0)
                {
                    return value?.ToString();
                }
                var doubleValue = System.Convert.ToDouble(StringToDoubleChecker.ConvertFromInvariantCulture(value?.ToString()));
                if (Math.Abs(doubleValue) < 0.1 / Math.Pow(10, Config.NumberOfDecimals - 1))
                {
                    return StringToDoubleChecker.ToCorrectFormat(doubleValue.ToString());
                }
                else
                {
                    string val = StringToDoubleChecker.ToCorrectFormat(doubleValue.ToString($"F{Config.NumberOfDecimals}"));
                    return val;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { StringToDoubleChecker.ConvertToDouble(StringToDoubleChecker.RemoveLetters(value.ToString(), out var q)) };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return StringToDoubleChecker.RemoveLetters(value.ToString(), out var cC);
            }
            catch
            {
                return value;
            }
        }
    }
}
