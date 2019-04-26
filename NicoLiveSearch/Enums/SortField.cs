
namespace NicoLiveSearch
{
    ///<summary>ソートのキーにするフィールド</summary>
    public enum SortField
    {
        ///<summary>放送者ID</summary>
        UserId,

        ///<summary>チャンネルID</summary>
        ChannelId,

        ///<summary>コミュニティID</summary>
        CommunityId,

        ///<summary>来場者数</summary>
        ViewCounter,

        ///<summary>コメント数</summary>
        CommentCounter,

        ///<summary>開場日時</summary>
        OpenTime,

        ///<summary>開始日時</summary>
        StartTime,

        ///<summary>タイムシフト予約者数</summary>
        ScoreTimeshiftReserved,
    }
}
