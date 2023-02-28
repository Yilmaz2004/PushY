using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PushY.Converter
{
    public class IdToBackgroundColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Id = App.UserId;

            Color Color = Color.DarkGreen;

            if (!string.IsNullOrEmpty(value.ToString()))
            {
                if (Id == (string)value)
                {
                    Color = Color.DarkCyan;
                }
                else
                {
                    Color = Color.DarkGreen;
                }
            }
            return Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
