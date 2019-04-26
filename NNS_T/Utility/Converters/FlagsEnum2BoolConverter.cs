using NicoLiveSearch;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    // xamlでCheckBoxとのbinding用
    ///<summary>検索対象フラグとboolの変換</summary>
    [ValueConversion(typeof(Targets), typeof(bool))]
    internal class Targets2CheckBoxConverter : FlagsEnum2BoolConverter<Targets> { }

    ///<summary>フラグenumとboolの変換</summary>
    internal class FlagsEnum2BoolConverter<T> : IValueConverter where T : Enum
    {
        private int target;

        /// <summary>フラグenum値がフラグを持っているかどうか</summary>
        /// <param name="value">対象フラグenum値</param>
        /// <param name="parameter">検証したいフラグ</param>
        /// <returns>bool</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is T)) return DependencyProperty.UnsetValue;
            if(!(parameter is T)) return DependencyProperty.UnsetValue;

            var v = (int)value;
            var p = (int)parameter;
            target = v;

            return (v & p) == p;
        }

        /// <summary>フラグenum値のフラグをOnOff</summary>
        /// <param name="value">bool</param>
        /// <param name="parameter">操作したいフラグ</param>
        /// <returns>合成後のフラグenum値</returns>
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
