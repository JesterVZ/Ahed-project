using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Ahed_project.MasterData
{
    public class Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return null;
            }
            string value = System.Convert.ToDecimal(values[0]?.ToString()?.Replace('.',',')).ToString($"F{Config.NumberOfDecimals}");
            return value;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { value };
        }
    }
}
