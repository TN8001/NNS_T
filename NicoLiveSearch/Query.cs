using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NicoLiveSearch
{
    ///<summary>niconico コンテンツ検索API(生放送) GETクエリパラメータ</summary>
    public class Query
    {
        ///<summary>検索キーワード (省略不可)</summary>
        public string Keyword { get; }

        ///<summary>検索対象 (省略不可)</summary>
        public Targets Targets { get; }

        /////<summary>レスポンスフィールド (省略可:totalCountだけ返ってくる)</summary>
        public ResponseFields Fields { get; set; }

        ///<summary>検索結果フィルタ (省略可)</summary>
        public FilterCollection Filters { get; set; }

        ///<summary>検索結果フィルタJson (省略可 URLエンコードする必要はありません)</summary>
        public string JsonFilter { get; set; }

        ///<summary>ソート順 (省略不可)</summary>
        public SortOrder Sort { get; }

        ///<summary>返ってくるコンテンツの取得オフセット (省略可:0)</summary>
        public int Offset { get; set; } = 0;

        ///<summary>返ってくるコンテンツの最大数 (省略可:10)</summary>
        public int Limit { get; set; } = 10;

        // 省略しても返ってくるようだが公式ドキュメントがそうなっているので。。。
        ///<summary>サービスまたはアプリケーション名 (省略不可)</summary>
        public string Context { get; }


        ///<summary>niconico コンテンツ検索API(生放送) GETクエリパラメータ</summary>
        /// <param name="keyword">検索キーワード</param>
        /// <param name="targets">検索対象</param>
        /// <param name="sort">ソート順</param>
        /// <param name="context">サービスまたはアプリケーション名</param>
        public Query(string keyword, Targets targets, SortOrder sort, string context)
        {
            Keyword = keyword ?? throw new ArgumentNullException(nameof(keyword));
            if(!targets.IsDefined()) throw new ArgumentOutOfRangeException(nameof(targets));
            Targets = targets;
            Sort = sort ?? throw new ArgumentNullException(nameof(sort));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        ///<summary>クエリ文字列</summary>
        public string ToQueryString()
            => string.Join("&", GetQueryDict().Select(x => $"{x.Key}={x.Value}"));

        ///<summary>エンコード済みクエリ文字列</summary>
        public string ToEncodeString()
        {
            using(var content = new FormUrlEncodedContent(GetQueryDict()))
                return content.ReadAsStringAsync().Result;
        }

        private Dictionary<string, string> GetQueryDict()
        {
            var d = new Dictionary<string, string>
            {
                { "q", Keyword },
                { "targets", Targets.ToQueryString() }
            };

            if(Fields != 0) d.Add("fields", Fields.ToQueryString());

            if(Filters != null)
                foreach(var kv in Filters.GetQueryPairs()) d.Add(kv.Key, kv.Value);

            if(JsonFilter != null)
            {
                var n = JsonFilter.Replace("\r\n", "\n").Replace("\r", "\n")
                                  .Split('\n').Select(s => s.Trim());
                d.Add("jsonFilter", Uri.EscapeDataString(string.Join("", n)));
            }

            d.Add("_sort", Sort.ToQueryString());
            if(Offset != 0) d.Add("_offset", Offset.ToString());
            if(Limit != 10) d.Add("_limit", Limit.ToString());
            d.Add("_context", Context);

            return d;
        }
    }
}
