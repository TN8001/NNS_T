using Newtonsoft.Json;

namespace NNS_T.Models.NicoAPI
{
    ///<summary>メタデータ</summary>
    [JsonObject]
    public class Meta
    {
        ///<summary>HTTPステータス (200等)</summary>
        [JsonProperty("status")]
        public int Status;

        ///<summary>ヒット件数</summary>
        [JsonProperty("totalCount")]
        public int TotalCount;

        ///<summary>リクエストID (54fbd5ff-df0c-42fd-8ddf-f64f73ad21b2等)</summary>
        [JsonProperty("id")]
        public string ID;

        ///<summary>エラーコード (QUERY_PARSE_ERROR等)</summary>
        [JsonProperty("errorCode")]
        public string ErrorCode;

        ///<summary>エラー内容 (query parse error等)</summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessage;
    }
}
