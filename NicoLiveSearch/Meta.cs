using Newtonsoft.Json;

namespace NicoLiveSearch
{
    ///<summary>メタデータ</summary>
    [JsonObject]
    public class Meta
    {
        ///<summary>HTTPステータス (200 等)</summary>
        [JsonProperty("status")]
        public int Status;

        ///<summary>ヒット件数</summary>
        [JsonProperty("totalCount")]
        public int TotalCount;

        ///<summary>リクエストID (英数字とハイフン36文字？)</summary>
        [JsonProperty("id")]
        public string Id;

        ///<summary>エラーコード (QUERY_PARSE_ERROR 等)</summary>
        [JsonProperty("errorCode")]
        public string ErrorCode;

        ///<summary>エラー内容 (query parse error 等)</summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessage;
    }
}
