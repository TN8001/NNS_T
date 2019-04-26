using System;
using System.Collections.Generic;
using System.Linq;

namespace NicoLiveSearch
{
    internal static class DictionaryExtensions
    {
#pragma warning disable IDE0034 // default' 式を単純化する appveyor C#7.0
        /// <summary>値を取得 keyがなければデフォルト値を取得</summary>
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
#pragma warning restore IDE0034 // default' 式を単純化する
        {
            if(dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if(key == null) throw new ArgumentNullException(nameof(key));

            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }

    internal static class EnumExtensions
    {
        ///<summary>定義されているかどうか Flags対応版</summary>
        public static bool IsDefined(this Enum value)
        {
            if(Attribute.IsDefined(value.GetType(), typeof(FlagsAttribute)))
            {
                var firstDigit = value.ToString()[0];
                return !char.IsDigit(firstDigit) && firstDigit != '-';
            }

            return Enum.IsDefined(value.GetType(), value);
        }

        ///<summary>ロワーキャメルケースに Flags対応版</summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static string ToLowerCamelCaseString(this Enum value)
        {
            if(!value.IsDefined()) throw new ArgumentOutOfRangeException(nameof(value));

            var l = value.ToString()
                         .Split(',')
                         .Select(s => s.Trim())
                         .Select(s => char.ToLowerInvariant(s[0]) + s.Substring(1));
            return string.Join(",", l);
        }

        // 使ってないし えらい雑
        ///<summary>スネークケースに Flags対応版</summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static string ToSnakeCaseString(this Enum value)
        {
            if(!value.IsDefined()) throw new ArgumentOutOfRangeException(nameof(value));

            var l = value.ToString()
                         .ToCharArray()
                         .Select(x => (char.IsUpper(x) ? "_" : "") + x.ToString());
            return string.Concat(l).ToLower().TrimStart('_').Replace(" ", "").Replace(",_", ",");
        }
    }
}
