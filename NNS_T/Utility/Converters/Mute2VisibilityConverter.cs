using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>ミュートを非表示用</summary>
    public class Mute2VisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Count() < 2) return Visibility.Visible;
            if(!(values[0] is bool)) return Visibility.Visible;
            if(!(values[1] is bool)) return Visibility.Visible;

            var b = (bool)values[0] & (bool)values[1];
            return b ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
