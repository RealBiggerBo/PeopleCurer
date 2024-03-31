using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.CustomValueConverters
{
    internal class DictionaryToPathData : IValueConverter
    {
        //const string basePath = "M 0,50 H 70 L 69,49 69,51 70,50 M 0,50 V 0 L -1,1 1,1 0,0 M -1,45 H 1 M -1,40 H 1 M -1,35 H 1 M -1,30 H 1 M -1,25 H 1 M -1,20 H 1 M -1,15 H 1 M -1,10 H 1 M -1,5 H 1 M 10,51 V 49 M 20,51 V 49 M 30,51 V 49 M 40,51 V 49 M 50,51 V 49 M 60,51 V 49";
        //const string example = "M 0,50 H 70 L 69,49 69,51 70,50 M 0,50 V 0 L -1,1 1,1 0,0 M -1,45 H 1 M -1,40 H 1 M -1,35 H 1 M -1,30 H 1 M -1,25 H 1 M -1,20 H 1 M -1,15 H 1 M -1,10 H 1 M -1,5 H 1 M 10,51 V 49 M 20,51 V 49 M 30,51 V 49 M 40,51 V 49 M 50,51 V 49 M 60,51 V 49 L 0,25 L 10,15 L 20,10 L 30,20 L 40,40 L 50,35 L 60,25";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Dictionary<DateTime, int> data)
            {
                PathFigureCollection figureCollection = new PathFigureCollection();
                PathFigureCollectionConverter.ParseStringToPathFigureCollection(figureCollection, GetDataString(data));
                PathGeometry dataGeometry = new PathGeometry(figureCollection);
                return dataGeometry;
            }
            return null;
        }

        private static string GetDataString(Dictionary<DateTime, int> data)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                    builder.Append(" M ");
                else
                    builder.Append(" L ");
                builder.Append((i+1) * 25 + 100);
                builder.Append(',');
                builder.Append(200 - data.ElementAt(i).Value * 1.8f);
            }

            return builder.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
