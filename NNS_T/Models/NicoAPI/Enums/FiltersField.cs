
namespace NNS_T.Models.NicoAPI
{
    // FlagsではないがFieldsに数値を合わせた
    ///<summary>フィルター対象フィールド</summary>
    public enum FiltersField
    {
        ///<summary>タグ</summary>
        Tags = 1 << 3,
        ///<summary>カテゴリタグ</summary>
        CategoryTags = 1 << 4,
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
        ///<summary>放送ステータス (生放送のみ)</summary>
        LiveStatus = 1 << 14,
    }
}
