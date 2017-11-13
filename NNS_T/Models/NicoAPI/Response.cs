using Newtonsoft.Json;

namespace NNS_T.Models.NicoAPI
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
