using System;

namespace NicoLiveSearch
{
    ///<summary>[Flags] レスポンスに要求するフィールド</summary>
    [Flags]
    public enum ResponseFields
    {
        ///<summary>コンテンツID</summary>
        ContentId = 1,

        ///<summary>タイトル</summary>
        Title = 1 << 1,

        ///<summary>説明文</summary>
        Description = 1 << 2,

        ///<summary>放送者ID</summary>
        UserId = 1 << 3,

        ///<summary>チャンネルID</summary>
        ChannelId = 1 << 4,

        ///<summary>コミュニティID</summary>
        CommunityId = 1 << 5,

        ///<summary>放送元種別</summary>
        ProviderType = 1 << 6,

        ///<summary>タグ(空白区切り)</summary>
        Tags = 1 << 7,

        ///<summary>来場者数</summary>
        ViewCounter = 1 << 8,

        ///<summary>コメント数</summary>
        CommentCounter = 1 << 9,

        ///<summary>開場日時</summary>
        OpenTime = 1 << 10,

        ///<summary>開始日時</summary>
        StartTime = 1 << 11,

        ///<summary>タイムシフト視聴可能か</summary>
        TimeshiftEnabled = 1 << 12,

        ///<summary>タイムシフト予約者数</summary>
        ScoreTimeshiftReserved = 1 << 13,

        ///<summary>サムネイルURL</summary>
        ThumbnailUrl = 1 << 14,

        ///<summary>コミュニティ名</summary>
        CommunityText = 1 << 15,

        ///<summary>コミュニティアイコンURL</summary>
        CommunityIcon = 1 << 16,

        ///<summary>チャンネル・コミュニティ限定か</summary>
        MemberOnly = 1 << 17,

        ///<summary>放送ステータス</summary>
        LiveStatus = 1 << 18,

        // Allがあると ToLowerCamelCaseStringが使えなくなってしまう...
        /////<summary>すべて</summary>
        //All = (1 << 19) - 1,
    }

    internal static class ResponseFieldsExtensions
    {
        ///<summary>Query用の文字列</summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static string ToQueryString(this ResponseFields self)
            => self.ToLowerCamelCaseString();
    }
}
