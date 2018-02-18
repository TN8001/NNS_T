//using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
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
        // Web版検索ページエントリポイント
        private const string WebUrl = "http://live.nicovideo.jp/search?";
        private HttpClient httpClient = new HttpClient();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appName"></param>
        public NicoApi(string appName = null)
        {
            httpClient.Timeout = TimeSpan.FromMilliseconds(10000);
            if(appName != null)
                httpClient.DefaultRequestHeaders.Add("User-Agent", appName);
        }

        ///<summary>Web版生放送検索ページURLを取得</summary>
        /// <param name="query">検索パラメータ</param>
        /// <param name="muteOfficial">公式を除外するか</param>
        public string GetSearchUrl(Query query, bool muteOfficial)
        {
            if(query == null) throw new ArgumentNullException(nameof(query));

            return WebUrl + query.GetSearchString(muteOfficial);
        }

        ///<summary>検索結果取得</summary>
        /// <param name="service">サービス種別（生以外未検証）</param>
        /// <param name="query">検索パラメータ</param>
        public async Task<Response> GetResponseAsync(Services service, Query query)
        {
            if(!Enum.IsDefined(typeof(Services), service))
                throw new ArgumentException(nameof(service));
            if(query == null) throw new ArgumentNullException(nameof(query));

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

        ///<summary>部屋名を取得</summary>
        /// <param name="roomID">部屋ID（co12345 ch12345）</param>
        public async Task<string> GetRoomNameAsync(string roomID)
        {
            if(roomID == null) throw new ArgumentNullException(nameof(roomID));

            try
            {
                if(roomID.StartsWith("co")) return await GetCommunityNameAsync(roomID);
                if(roomID.StartsWith("ch")) return await GetChannelNameAsync(roomID);
                throw new ArgumentException(nameof(roomID));
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

        private static Regex reg = new Regex(@"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private async Task<string> GetTitleAsync(string url)
        {
            // フォロワーのみになっていると404になるのでGetAsync
            var response = await httpClient.GetAsync(url);
            var s = await response.Content.ReadAsStringAsync();

            // タイトル以外に使うことがなさそうなのでAngleSharpを外した
            //var parser = new HtmlParser();
            //var doc = await parser.ParseAsync(s);

            //return doc.Title;

            return reg.Match(s).Groups["Title"].Value.Trim();
        }
    }
}
