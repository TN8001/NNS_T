using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NicoLiveSearch
{
    ///<summary>生放送情報</summary>
    [JsonObject]
    public class Datum
    {
        private static readonly string communityUrl = "https://com.nicovideo.jp/community/co";
        private static readonly string channelUrl = "https://ch.nicovideo.jp/ch";
        private static readonly string userPageUrl = "https://www.nicovideo.jp/user/";
        private static readonly string contentsUrl = "https://nico.ms/";

        ///<summary>チャンネル or コミュニティページURL</summary>
        public string RoomUrl => CommunityPageUrl ?? ChannelPageUrl;

        ///<summary>チャンネルページURL</summary>
        public string ChannelPageUrl => ChannelId != null ? channelUrl + ChannelId : null;

        ///<summary>コミュニティページURL</summary>
        public string CommunityPageUrl => CommunityId != null ? communityUrl + CommunityId : null;

        ///<summary>ユーザーページURL</summary>
        public string UserPageUrl => UserId != null ? userPageUrl + UserId : null;

        ///<summary>放送ページURL</summary>
        public string LiveUrl => contentsUrl + ContentId;

        ///<summary>部屋ID (co1234567やch1234 等)</summary>
        public string RoomId => CommunityId != null ? "co" + CommunityId
                                : ChannelId != null ? "ch" + ChannelId
                                                    : null;

        ///<summary>ID (lv123456789 等)</summary>
        [JsonProperty("contentId")]
        public string ContentId;

        ///<summary>タイトル</summary>
        [JsonProperty("title")]
        public string Title;

        ///<summary>説明文 (htmlタグ、改行コード含む)</summary>
        [JsonProperty("description")]
        public string Description;

        ///<summary>放送者ID (公式放送ではnull)</summary>
        [JsonProperty("userId")]
        public int? UserId;
        ///<summary>チャンネルID (ユーザー放送ではnull)</summary>
        [JsonProperty("channelId")]
        public int? ChannelId;

        ///<summary>コミュニティID (ユーザー放送でなければnull)</summary>
        [JsonProperty("communityId")]
        public int? CommunityId;

        ///<summary>放送元種別</summary>
        [JsonProperty("providerType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProviderType ProviderType;

        ///<summary>タグ (空白区切り)</summary>
        [JsonProperty("tags")]
        public string Tags;

        ///<summary>来場者数</summary>
        [JsonProperty("viewCounter")]
        public int ViewCounter;

        ///<summary>コメント数</summary>
        [JsonProperty("commentCounter")]
        public int CommentCounter;

        ///<summary>開場日時 (2017-01-01T01:01:01+09:00 等)</summary>
        [JsonProperty("openTime")]
        public DateTime OpenTime;

        ///<summary>開始日時 (2017-01-01T01:01:01+09:00 等)</summary>
        [JsonProperty("startTime")]
        public DateTime StartTime;

        ///<summary>タイムシフト視聴可能か</summary>
        [JsonProperty("timeshiftEnabled")]
        public bool TimeshiftEnabled;

        ///<summary>タイムシフト予約数</summary>
        [JsonProperty("scoreTimeshiftReserved")]
        public int ScoreTimeshiftReserved;

        ///<summary>サムネイルURL (ユーザー放送ではnull)</summary>
        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl;

        ///<summary>コミュニティ名 (公式放送ではnull)</summary>
        [JsonProperty("communityText")]
        public string CommunityText;

        ///<summary>コミュニティアイコンURL (公式放送ではnull)</summary>
        [JsonProperty("communityIcon")]
        public string CommunityIcon;

        ///<summary>チャンネル・コミュニティ限定か</summary>
        [JsonProperty("memberOnly")]
        public bool MemberOnly;

        ///<summary>放送ステータス</summary>
        [JsonProperty("liveStatus")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LiveStatus LiveStatus;
    }
}
