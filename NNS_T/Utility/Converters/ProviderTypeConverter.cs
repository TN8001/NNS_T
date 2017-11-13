using NNS_T.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>放送者種別を縦書きにｗ</summary>
    public class ProviderTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ProviderType type)
            {
                return type == ProviderType.Official ? "公\n\n式"
                      : type == ProviderType.Channel ? "チ\nャ\nン\nネ\nル"
                                                     : "ユ\n｜\nザ\n｜";
            }

            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
