using Newtonsoft.Json;
using System;

namespace NNS_T.Models.NicoAPI
{
    ///<summary>コンテンツ情報</summary>
    [JsonObject]
    public class Datum
    {
        ///<summary>コンテンツID (lv123456789等)</summary>
        [JsonProperty("contentId")]
        public string ContentID;

        ///<summary>タイトル</summary>
        [JsonProperty("title")]
        public string Title;

        ///<summary>説明文 (htmlタグ、改行コード含む)</summary>
        [JsonProperty("description")]
        public string Description;

        ///<summary>タグ (空白区切り)</summary>
        [JsonProperty("tags")]
        public string Tags;

        ///<summary>カテゴリタグ (空白区切り？)</summary>
        [JsonProperty("categoryTags")]
        public string CategoryTags;

        ///<summary>再生、来場数</summary>
        [JsonProperty("viewCounter")]
        public int ViewCounter;

        ///<summary>マイリスト数 (生放送では使用不可)</summary>
        [JsonProperty("mylistCounter")]
        public int MylistCounter;

        ///<summary>コメント数</summary>
        [JsonProperty("commentCounter")]
        public int CommentCounter;

        ///<summary>投稿、放送開始時間 (2017-01-01T01:01:01+09:00等)</summary>
        [JsonProperty("startTime")]
        public DateTime StartTime;

        ///<summary>最新コメント時間 (生放送では使用不可)</summary>
        [JsonProperty("lastCommentTime")]
        public DateTime LastCommentTime;

        ///<summary>再生時間(秒) (生放送では使用不可)</summary>
        [JsonProperty("lengthSeconds")]
        public int LengthSeconds;

        ///<summary>サムネイルURL (ユーザー放送ではnull)</summary>
        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl;

        ///<summary>コミュニティアイコンURL (公式放送ではnull？)</summary>
        [JsonProperty("communityIcon")]
        public string CommunityIcon;

        ///<summary>タイムシフト予約数 (生放送のみ)</summary>
        [JsonProperty("scoreTimeshiftReserved")]
        public int ScoreTimeshiftReserved;

        ///<summary>放送種別 (生放送のみ 'past'=終了,'onair'=放送中,'reserved'=予約)</summary>
        [JsonProperty("liveStatus")]
        public string LiveStatus;
    }
}
