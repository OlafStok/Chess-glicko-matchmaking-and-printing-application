using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace De_7_Pionnen
{
    class DividerRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            switch (input)
            {
                case "Ronde 1":
                    return Brushes.LightBlue;
                default:
                    return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
