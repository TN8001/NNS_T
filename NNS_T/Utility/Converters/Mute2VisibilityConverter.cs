using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    // ひどすぎんよ。。。
    ///<summary>アイテム表示非表示用</summary>
    internal class Mute2VisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var mute_HideList = (bool)values[0];
            var item_IsMuted = (bool)values[1];
            if(item_IsMuted && mute_HideList) return Visibility.Collapsed;

            var search_HideMemberOnly = (bool)values[2];
            var item_MemberOnly = (bool)values[3];
            if(item_MemberOnly && search_HideMemberOnly) return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
