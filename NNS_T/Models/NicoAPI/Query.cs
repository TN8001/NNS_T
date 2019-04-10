using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NNS_T.Models.NicoAPI
{
    // 一応汎用になっているが生放送検索以外未検証
    ///<summary>niconico コンテンツ検索API GETクエリパラメータ</summary>
    public class Query
    {
        ///<summary>検索キーワード (省略不可)</summary>
        public string Keyword;

        ///<summary>検索対象 (省略不可)</summary>
        public Targets Targets;

        /////<summary>レスポンスフィールド (省略可:totalCountだけ返ってくる)</summary>
        public Fields Fields;

        ///<summary>検索結果フィルタ (省略可)</summary>
        public FilterCollection Filters;

        // UIは対応しないが設定は許可
        ///<summary>検索結果フィルタJson (省略可)</summary>
        public string JsonFilter;

        ///<summary>ソート順 (省略不可)</summary>
        public Sort Sort;

        ///<summary>返ってくるコンテンツの取得オフセット (省略可:0)</summary>
        public int Offset = 0;

        ///<summary>返ってくるコンテンツの最大数 (省略可:10)</summary>
        public int Limit = 10;

        // 省略しても返ってくるようだが公式ドキュメントがそうなっているので。。。
        ///<summary>サービスまたはアプリケーション名 (省略不可)</summary>
        public string Context;

        /// <summary>niconico コンテンツ検索API GETクエリパラメータ</summary>
        /// <param name="keyword">検索キーワード</param>
        /// <param name="targets">検索対象</param>
        /// <param name="sort">ソート順</param>
        /// <param name="context">サービスまたはアプリケーション名</param>
        public Query(string keyword, Targets targets, Sort sort, string context)
            => (Keyword, Targets, Sort, Context) = (keyword, targets, sort, context);

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

            foreach(var kv in Filters.GetQueryPairs()) d.Add(kv.Key, kv.Value);

            if(JsonFilter != null) d.Add("jsonFilter", JsonFilter);

            d.Add("_sort", Sort.ToQueryString());
            if(Offset != 0) d.Add("_offset", Offset.ToString());
            if(Limit != 10) d.Add("_limit", Limit.ToString());
            d.Add("_context", Context);

            return d;
        }
    }
}
