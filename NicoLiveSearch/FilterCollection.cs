using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NicoLiveSearch
{
    public class FilterCollection : Collection<IFilter>
    {
        // filters[tags][0]等の数字の部分 無くても([])同じ数字でも リクエストは通るようだが
        // こちらの実装上辞書を通るので真面目に生成
        ///<summary>filtersをKeyValuePairで返す</summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public IEnumerable<KeyValuePair<string, string>> GetQueryPairs()
        {
            var d = new Dictionary<string, int>();
            foreach(var filter in Items)
            {
                var k = filter.Field.ToLowerCamelCaseString();
                var o = filter.Op;
                var i = o.ToString().ToLowerInvariant();
                if(o == CompOp.EQ)
                {
                    var v = d.GetOrDefault(k);
                    i = v.ToString();
                    d[k] = v + 1;
                }
                yield return new KeyValuePair<string, string>($"filters[{k}][{i}]", filter.Value);
            }
        }
    }
}
