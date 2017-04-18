using MyExpenses.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyExpenses.Converters
{

    public class EnumToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            switch ((Category)value)
            {
                case Category.Flight:
                    return "Flight.png";
                case Category.Hotel:
                    return "Hotel.png";
                case Category.Meal:
                    return "Restaurant.png";
                case Category.Other:
                    return "Description.png";
                case Category.Taxi:
                    return "Taxi.png";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
