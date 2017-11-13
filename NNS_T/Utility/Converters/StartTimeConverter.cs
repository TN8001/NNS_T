using System;
using System.Globalization;
using System.Windows.Data;

namespace NNS_T.Utility.Converters
{
    ///<summary>経過時間付き日時フォーマット</summary>
    public class StartTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime time)
            {
                var t = time.ToString("yyyy/MM/dd(ddd)HH:mm 開始 ");
                var s = (DateTime.Now - time).ToString(@"h'時間'm'分経過'");
                if(s.StartsWith("0時間")) s = s.Replace("0時間", "");
                return t + s;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
