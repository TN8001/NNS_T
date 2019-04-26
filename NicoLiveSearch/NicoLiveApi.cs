using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NicoLiveSearch
{
    ///<summary>niconico生放送検索</summary>
    public class NicoLiveApi
    {
        ///<summary>コミュニティURL</summary>
        public static string CommunityUrl { get; } = "https://com.nicovideo.jp/community/";
        ///<summary>チャンネルURL</summary>
        public static string ChannelUrl { get; } = "https://ch.nicovideo.jp/";

        // APIエントリポイント
        private static readonly string apiUrl = "https://api.search.nicovideo.jp/api/v2/live/contents/search?";
        // Web版検索ページエントリポイント
        private static readonly string webUrl = "https://live.nicovideo.jp/search?";

        private readonly HttpClient httpClient; // = new HttpClient();

        ///<summary>niconico生放送検索</summary>
        /// <param name="client">DI HttpClient</param>
        /// <exception cref="ArgumentNullException"></exception> 
        public NicoLiveApi(HttpClient client)
            => httpClient = client ?? throw new ArgumentNullException(nameof(client));

        ///<summary>niconico生放送検索</summary>
        /// <param name="appName">User-Agentに設定するアプリ名</param>
        /// <exception cref="ArgumentNullException"></exception> 
        public NicoLiveApi(string appName)
        {
            if(appName == null) throw new ArgumentNullException(nameof(appName));

            httpClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(10000) };
            httpClient.DefaultRequestHeaders.Add("User-Agent", appName);
        }


        ///<summary>Web版生放送検索ページURLを取得</summary>
        /// <param name="query">検索パラメータ</param>
        /// <param name="muteOfficial">公式を除外するか</param>
        /// <exception cref="ArgumentNullException"></exception> 
        public string GetSearchUrl(Query query, bool muteOfficial)
        {
            if(query == null) throw new ArgumentNullException(nameof(query));

            return webUrl + GetSearchString(query, muteOfficial);


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
        /// <param name="query">検索パラメータ</param>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        /// <exception cref="NicoApiRequestException"></exception> 
        public async Task<Response> GetResponseAsync(Query query)
            => await GetResponseAsync(query, CancellationToken.None);

        ///<summary>検索結果取得</summary>
        /// <param name="query">検索パラメータ</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException"></exception> 
        /// <exception cref="NicoApiRequestException"></exception> 
        /// <exception cref="OperationCanceledException"></exception> 
        public async Task<Response> GetResponseAsync(Query query, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"GetResponseAsync:{cancellationToken.GetHashCode()}");
            if(query == null) throw new ArgumentNullException(nameof(query));

            try
            {
                var s = await GetHttpStringAsync(apiUrl + query.ToEncodeString(), cancellationToken);
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
            catch(NicoApiRequestException)
            {
                throw;
            }
            catch(OperationCanceledException e)
            {
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
        private async Task<string> GetHttpStringAsync(string encodeUri, CancellationToken cancellationToken)
        {
            // エラーコード取得のためGetAsync
            var response = await httpClient.GetAsync(encodeUri, cancellationToken);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
