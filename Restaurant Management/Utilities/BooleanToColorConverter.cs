using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Restaurant_Management.Utilities
{
    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool status && status)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98DFD6")); // Màu cho trạng thái true
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDD83")); // Màu cho trạng thái false
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
