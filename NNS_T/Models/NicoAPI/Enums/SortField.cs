
namespace NNS_T.Models.NicoAPI
{
    // FlagsではないがFieldsに数値を合わせた
    ///<summary>ソートのキーにするフィールド</summary>
    public enum SortField
    {
        ///<summary>再生数</summary>
        ViewCounter = 1 << 5,
        ///<summary>マイリスト数 (生放送不可)</summary>
        MylistCounter = 1 << 6,
        ///<summary>コメント数</summary>
        CommentCounter = 1 << 7,
        ///<summary>投稿日時</summary>
        StartTime = 1 << 8,
        ///<summary>最新コメント日時 (生放送不可)</summary>
        LastCommentTime = 1 << 9,
        ///<summary>再生時間 (生放送不可)</summary>
        LengthSeconds = 1 << 10,
        ///<summary>タイムシフト予約者数 (生放送のみ)</summary>
        ScoreTimeshiftReserved = 1 << 13,
    }
}
