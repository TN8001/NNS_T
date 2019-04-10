using System;
using System.Collections.Generic;
using System.Linq;

namespace NNS_T.Models.NicoAPI
{
    internal static class DictionaryExtensions
    {
        /// <summary>値を取得 keyがなければデフォルト値を取得</summary>
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if(dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if(key == null) throw new ArgumentNullException(nameof(key));

            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }

    internal static class EnumExtensions
    {
        ///<summary>ロワーキャメルケースに</summary>
        public static string ToLowerCamelCaseString(this Enum value)
        {
            if(!Enum.IsDefined(value.GetType(), value)) throw new ArgumentOutOfRangeException(nameof(value));

            var s = value.ToString();
            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }

        ///<summary>スネークケースに</summary>
        public static string ToSnakeCaseString(this Enum value)
        {
            if(!Enum.IsDefined(value.GetType(), value)) throw new ArgumentOutOfRangeException(nameof(value));

            var e = value.ToString()
                   .Select(x => (char.IsUpper(x) ? "_" : "") + x.ToString());
            return string.Concat(e).ToLower().TrimStart('_');
        }
    }
}
