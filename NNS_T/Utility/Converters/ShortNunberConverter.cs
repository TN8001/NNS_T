using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>数値をフォーマット 1万以上概数</summary>
    [ValueConversion(typeof(int), typeof(string))]
    internal class ShortNunberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int i)
            {
                if(i < 10_000) return i.ToString("#,0");
                return (i / 10_000f).ToString("#,0.#万");
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
