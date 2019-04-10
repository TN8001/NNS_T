using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>valueがparameterならVisible（flags未対応）</summary>
    internal class EnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null || parameter == null) return Visibility.Collapsed;
            if(!(value is Enum) || !(parameter is Enum)) return Visibility.Collapsed;
            if(value.GetType() != parameter.GetType()) return Visibility.Collapsed;

            return (int)value == (int)parameter ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
