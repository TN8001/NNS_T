using System;
using System.Globalization;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>value > parameter パラメーターを超えている場合true</summary>
    [ValueConversion(typeof(int), typeof(bool))]
    public class IsGreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (int)value > int.Parse((string)parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
