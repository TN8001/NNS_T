
namespace NicoLiveSearch
{
    ///<summary>フィルター対象フィールド</summary>
    public enum FiltersField
    {
        ///<summary>放送者ID</summary>
        UserId,

        ///<summary>チャンネルID</summary>
        ChannelId,

        ///<summary>コミュニティID</summary>
        CommunityId,

        ///<summary>放送元種別</summary>
        ProviderType,

        ///<summary>タグ</summary>
        Tags,

        ///<summary>タグ完全一致</summary>
        TagsExact,

        ///<summary>来場者数</summary>
        ViewCounter,

        ///<summary>コメント数</summary>
        CommentCounter,

        ///<summary>開場日時</summary>
        OpenTime,

        ///<summary>開始日時</summary>
        StartTime,

        ///<summary>タイムシフト視聴可能か</summary>
        TimeshiftEnabled,

        ///<summary>タイムシフト予約者数</summary>
        ScoreTimeshiftReserved,

        ///<summary>コミュニティ名</summary>
        CommunityText,

        ///<summary>チャンネル・コミュニティ限定か</summary>
        MemberOnly,

        ///<summary>放送ステータス</summary>
        LiveStatus,
    }
}
