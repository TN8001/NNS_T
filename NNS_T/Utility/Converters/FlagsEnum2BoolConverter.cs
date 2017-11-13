using NNS_T.Models.NicoAPI;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>検索対象フラグとboolの変換</summary>
    public class Targets2CheckBoxConverter : FlagsEnum2BoolConverter<Targets> { }

    ///<summary>フラグenumとboolの変換</summary>
    public class FlagsEnum2BoolConverter<T> : IValueConverter where T : struct, IConvertible
    {
        private int target;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is T)) return DependencyProperty.UnsetValue;
            if(!(parameter is T)) return DependencyProperty.UnsetValue;

            var v = (int)value;
            var p = (int)parameter;
            target = v;

            return (v & p) == p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is bool)) return DependencyProperty.UnsetValue;
            if(!(parameter is T)) return DependencyProperty.UnsetValue;

            var p = (int)parameter;

            if((bool)value) target |= p;
            else target &= ~p;

            return (T)Enum.ToObject(typeof(T), target);
        }
    }
}
