using Newtonsoft.Json;

namespace NicoLiveSearch
{
    ///<summary>レスポンスデータ</summary>
    [JsonObject]
    public class Response
    {
        ///<summary>メタデータ</summary>
        [JsonProperty("meta")]
        public Meta Meta;

        ///<summary>アイテムデータ</summary>
        [JsonProperty("data")]
        public Datum[] Data;
    }
}
