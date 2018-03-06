using System;
using System.Globalization;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>DateTimeから経過時間のみの文字列</summary>
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class StartTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime time)
            {
                var ts = (DateTime.Now - time);
                var s = $"{(int)ts.TotalHours}時間{ts.Minutes}分経過";
                if(s.StartsWith("0時間")) s = s.Replace("0時間", "");
                return s;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
