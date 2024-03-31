using PeopleCurer.Models;
using PeopleCurer.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.CustomValueConverters
{
    internal class DoubleToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is double d)
            {
                return Math.Floor(d).ToString();
            }
            return 50;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is string s)
            {
                if (double.TryParse(s, out double d))
                {
                    return d;
                }
            }
            return "50";
        }
    }
}
