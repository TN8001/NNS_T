using System.Collections.Generic;
using System.Collections.Specialized;
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

        ///<summary>検索方法 (省略不可)</summary>
        public Targets Targets;

        /////<summary>レスポンスフィールド (省略可:totalCountだけ返ってくる)</summary>
        public Fields Fields;

        ///<summary>検索結果フィルタ (省略可)</summary>
        public NameValueCollection Filters;

        // UIまわりが面倒すぎるので対応しない
        /////<summary>検索結果フィルタJson (省略可)</summary>
        //public string JsonFilter;

        ///<summary>ソート順 (省略不可)</summary>
        public string Sort;

        ///<summary>返ってくるコンテンツの取得オフセット (省略可:0)</summary>
        public int Offset = 0;

        ///<summary>返ってくるコンテンツの最大数 (省略可:10)</summary>
        public int Limit = 10;

        ///<summary>サービスまたはアプリケーション名 (省略不可)</summary>
        public string Context;

        ///<summary>クエリ文字列</summary>
        public override string ToString()
            => string.Join("&", GetQueryDict().Select(x => $"{x.Key}={x.Value}"));

        ///<summary>エンコード済みクエリ文字列</summary>
        public string ToEncodeString()
        {
            using(var content = new FormUrlEncodedContent(GetQueryDict()))
                return content.ReadAsStringAsync().Result;
        }

        private Dictionary<string, string> GetQueryDict()
        {
            var d = new Dictionary<string, string>();
            d.Add("q", Keyword);
            d.Add("targets", Targets.ToStringEx());
            if(Fields != 0) d.Add("fields", Fields.ToStringEx());

            foreach(var key in Filters.AllKeys)
            {
                var values = Filters.GetValues(key);
                for(var i = 0; i < values.Length; i++)
                    d.Add($"filters[{key}][{i}]", values[i]);
            }

            d.Add("_sort", Sort);
            if(Offset != 0) d.Add("_offset", Offset.ToString());
            if(Limit != 10) d.Add("_limit", Limit.ToString());
            d.Add("_context", Context);

            return d;
        }
    }
}
