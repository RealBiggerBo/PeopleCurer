using PeopleCurer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.CustomValueConverters
{
    internal class ColorThemeToColorConverter : IValueConverter
    {
        public Color BlueThemeDarkColor { get; set; }
        public Color BlueThemeMidColor { get; set; }
        public Color BlueThemeLightColor { get; set; }

        public Color RedThemeDarkColor { get; set; }
        public Color RedThemeMidColor { get; set; }
        public Color RedThemeLightColor { get; set; }

        public Color GreenThemeDarkColor { get; set; }
        public Color GreenThemeMidColor { get; set; }
        public Color GreenThemeLightColor { get; set; }

        public Color PurpleThemeDarkColor { get; set; }
        public Color PurpleThemeMidColor { get; set; }
        public Color PurpleThemeLightColor { get; set; }

        public Color YellowThemeDarkColor { get; set; }
        public Color YellowThemeMidColor { get; set; }
        public Color YellowThemeLightColor { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ColorTheme theme)
            {
                if (parameter is string shade)
                {
                    if (shade.Equals("Dark", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return theme switch
                        {
                            ColorTheme.Blue => BlueThemeDarkColor,
                            ColorTheme.Green => GreenThemeDarkColor,
                            ColorTheme.Red => RedThemeDarkColor,
                            ColorTheme.Purple => PurpleThemeDarkColor,
                            ColorTheme.Yellow => YellowThemeDarkColor,
                            _ => BlueThemeDarkColor,
                        };
                    }
                    else if(shade.Equals("mid", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return theme switch
                        {
                            ColorTheme.Blue => BlueThemeMidColor,
                            ColorTheme.Green => GreenThemeMidColor,
                            ColorTheme.Red => RedThemeMidColor,
                            ColorTheme.Purple => PurpleThemeMidColor,
                            ColorTheme.Yellow => YellowThemeMidColor,
                            _ => BlueThemeMidColor,
                        };
                    }
                    else if (shade.Equals("light", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return theme switch
                        {
                            ColorTheme.Blue => BlueThemeLightColor,
                            ColorTheme.Green => GreenThemeLightColor,
                            ColorTheme.Red => RedThemeLightColor,
                            ColorTheme.Purple => PurpleThemeLightColor,
                            ColorTheme.Yellow => YellowThemeLightColor,
                            _ => BlueThemeLightColor,
                        };
                    }
                }
            }
            return BlueThemeDarkColor;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
