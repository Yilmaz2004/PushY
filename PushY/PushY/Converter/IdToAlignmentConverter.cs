using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PushY.Converter
{
    public class IdToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Id = App.UserId;

            LayoutOptions Alignment = LayoutOptions.Start;

            if (!string.IsNullOrEmpty(value.ToString()))
            {
                if (Id == (string)value)
                {
                    Alignment = LayoutOptions.End;
                }
                else
                {
                    Alignment = LayoutOptions.Start;
                }
            }
            return Alignment;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
