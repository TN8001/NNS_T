using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NNS_T.Models.NicoAPI
{
    ///<summary>niconicoコンテンツ検索API 取得エラー</summary>
    public class NicoApiRequestException : HttpRequestException
    {
        public NicoApiRequestException(string message) : base(message) { }
        public NicoApiRequestException(string message, Exception innerException) : base(message, innerException) { }
    }

    // 一応汎用になっているが生放送検索以外未検証
    ///<summary>niconicoコンテンツ検索API</summary>
    public class NicoApi
    {
        ///<summary>コミュニティURL</summary>
        public const string CommunityUrl = "http://com.nicovideo.jp/community/";
     
        ///<summary>チャンネルURL</summary>
        public const string ChannelUrl = "http://com.nicovideo.jp/channel/";

        // APIエントリポイント
        private const string ApiUrl = "http://api.search.nicovideo.jp/api/v2/:service/contents/search?";

        private HttpClient httpClient = new HttpClient();

        public NicoApi(string appName = null)
        {
            httpClient.Timeout = TimeSpan.FromMilliseconds(10000);
            if(appName != null)
                httpClient.DefaultRequestHeaders.Add("User-Agent", appName);
        }

        ///<summary>検索結果取得</summary>
        public async Task<Response> GetResponseAsync(Services service, Query query)
        {
            var api = ApiUrl.Replace(":service", service.ToStringEx());
            try
            {
                var s = await GetHttpStringAsync(api + query.ToEncodeString());
                var r = JsonConvert.DeserializeObject<Response>(s);

                // エラーメッセージをViewの表示に使用中
                if(r.Meta.Status != 200)
                {
                    var m = r.Meta.Status == 400 ? "Status:400 不正なパラメータです。"
                          : r.Meta.Status == 500 ? "Status:500 検索サーバの異常です。"
                          : r.Meta.Status == 503 ? "Status:503 サービスがメンテナンス中です。"
                          : $"Status:{r.Meta.Status} {r.Meta.ErrorMessage}";

                    throw new NicoApiRequestException(m);
                }

                return r;
            }
            catch(TaskCanceledException e)
            {
                throw new NicoApiRequestException("タイムアウトしました。", e);
            }
            catch(HttpRequestException e)
            {
                throw new NicoApiRequestException("取得に失敗しました。", e);
            }
            catch(Exception e)
            {
                throw new NicoApiRequestException("不明なエラーです。", e);
            }
        }

        ///<summary>部屋の名前を取得</summary>
        public async Task<string> GetRoomNameAsync(string roomID)
        {
            if(roomID == null) throw new ArgumentNullException(nameof(roomID));

            try
            {
                if(roomID.StartsWith("co")) return await GetCommunityNameAsync(roomID);
                if(roomID.StartsWith("ch")) return await GetChannelNameAsync(roomID);
            }
            catch
            {
                return null;
            }

            throw new ArgumentException(nameof(roomID));
        }

        private async Task<string> GetHttpStringAsync(string encodeUri)
        {
            // エラーコード取得のためGetAsync
            var response = await httpClient.GetAsync(encodeUri);
            return await response.Content.ReadAsStringAsync();
        }
        private async Task<string> GetCommunityNameAsync(string communityID)
        {
            var t = await GetTitleAsync(CommunityUrl + communityID);
            return t.Replace("-ニコニコミュニティ", "");
        }
        private async Task<string> GetChannelNameAsync(string channelID)
        {
            var t = await GetTitleAsync(ChannelUrl + channelID);
            var i = t.IndexOf(" - ニコニコチャンネル");
            if(i < 0) i = t.Length;
            return t.Substring(0, i);
        }
        private async Task<string> GetTitleAsync(string url)
        {
            // フォロワーのみになっていると404になるのでGetAsync
            var response = await httpClient.GetAsync(url);
            var s = await response.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var doc = await parser.ParseAsync(s);
            return doc.Title;
        }
    }
}
