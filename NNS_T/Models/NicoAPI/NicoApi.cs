using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NNS_T.Models.NicoAPI
{
    // 一応汎用になっているが生放送検索以外未検証
    ///<summary>niconicoコンテンツ検索API</summary>
    public class NicoApi
    {
        ///<summary>コミュニティURL</summary>
        public const string CommunityUrl = "https://com.nicovideo.jp/community/";
        ///<summary>チャンネルURL</summary>
        public const string ChannelUrl = "https://ch.nicovideo.jp/";

        // APIエントリポイント
        private const string ApiUrl = "https://api.search.nicovideo.jp/api/v2/:service/contents/search?";
        // Web版検索ページエントリポイント
        private const string WebUrl = "https://live.nicovideo.jp/search?";
        private readonly HttpClient httpClient = new HttpClient();

        ///<summary>niconicoコンテンツ検索API</summary>
        /// <param name="appName">User-Agent</param>
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

            return WebUrl + GetSearchString(query, muteOfficial);


            string GetSearchString(Query q, bool m)
            {
                var d = new Dictionary<string, string>
                {
                    { "sort", "recent" },
                    { "keyword", q.Keyword }
                };
                if(m) d.Add("filter", ":onair:+:channel:+:community:");
                else d.Add("filter", ":onair:");

                if(q.Targets.HasFlag(Targets.TagsExact)) d.Add("kind", "tags");

                using(var content = new FormUrlEncodedContent(d))
                    return content.ReadAsStringAsync().Result;
            }
        }

        ///<summary>検索結果取得</summary>
        /// <param name="service">サービス種別（生以外未検証）</param>
        /// <param name="query">検索パラメータ</param>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        /// <exception cref="NicoApiRequestException"></exception> 
        public async Task<Response> GetResponseAsync(Services service, Query query)
            => await GetResponseAsync(service, query, CancellationToken.None);

        ///<summary>検索結果取得</summary>
        /// <param name="service">サービス種別（生以外未検証）</param>
        /// <param name="query">検索パラメータ</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        /// <exception cref="NicoApiRequestException"></exception> 
        /// <exception cref="OperationCanceledException"></exception> 
        public async Task<Response> GetResponseAsync(Services service, Query query, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"GetResponseAsync:{cancellationToken.GetHashCode()}");
            if(!Enum.IsDefined(typeof(Services), service))
                throw new ArgumentException(nameof(service));
            if(query == null) throw new ArgumentNullException(nameof(query));

            var api = ApiUrl.Replace(":service", service.ToSnakeCaseString());
            try
            {
                var s = await GetHttpStringAsync(api + query.ToEncodeString(), cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var r = JsonConvert.DeserializeObject<Response>(s);
                cancellationToken.ThrowIfCancellationRequested();

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
            catch(OperationCanceledException e)
            {
                Debug.Write("NicoApi Canceled:");
                if(cancellationToken.IsCancellationRequested)
                    throw;
                else // release buildだと来ない??
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
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        /// <exception cref="NicoApiRequestException"></exception> 
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

        private async Task<string> GetHttpStringAsync(string encodeUri, CancellationToken cancellationToken)
        {
            // エラーコード取得のためGetAsync
            var response = await httpClient.GetAsync(encodeUri, cancellationToken);
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

        private static readonly Regex reg = new Regex(@"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private async Task<string> GetTitleAsync(string url)
        {
            // フォロワーのみになっていると404になるのでGetAsync
            var response = await httpClient.GetAsync(url);
            var s = await response.Content.ReadAsStringAsync();

            return WebUtility.HtmlDecode(reg.Match(s).Groups["Title"].Value.Trim());
        }
    }
}
