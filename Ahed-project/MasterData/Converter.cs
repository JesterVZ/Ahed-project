using Ahed_project.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string value = StringToDoubleChecker.ToCorrectFormat(System.Convert.ToDecimal(values[0]?.ToString()?.Replace('.',',')).ToString($"F{Config.NumberOfDecimals}"));
            return value;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == DependencyProperty.UnsetValue)
                {
                    return null;
                }
                string val = StringToDoubleChecker.ToCorrectFormat(System.Convert.ToDecimal(value?.ToString()?.Replace('.', ',')).ToString($"F{Config.NumberOfDecimals}"));
                return val;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
            
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { StringToDoubleChecker.ConvertToDouble(StringToDoubleChecker.RemoveLetters(value.ToString(),out var q)) };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return StringToDoubleChecker.ConvertToDouble(value.ToString());
            }
            catch
            {
                return value;
            }
        }
    }
}
