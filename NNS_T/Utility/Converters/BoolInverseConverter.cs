﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>bool反転</summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    internal class BoolInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool b) return !b;

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool b) return !b;

            return DependencyProperty.UnsetValue;
        }
    }
}
