using System;
using System.Collections.Generic;

namespace NNS_T.Models.NicoAPI
{
    // 一応汎用になっているが生放送検索以外未検証
    ///<summary>[Flags] レスポンスに含むフィールド</summary>
    [Flags]
    public enum Fields
    {
        ///<summary>コンテンツID</summary>
        ContentId = 1,

        ///<summary>タイトル</summary>
        Title = 1 << 1,

        ///<summary>説明文</summary>
        Description = 1 << 2,

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

        ///<summary>サムネイルURL</summary>
        ThumbnailUrl = 1 << 11,

        ///<summary>コミュニティアイコンURL</summary>
        CommunityIcon = 1 << 12,

        ///<summary>タイムシフト予約者数 (生放送のみ)</summary>
        ScoreTimeshiftReserved = 1 << 13,

        ///<summary>放送ステータス (生放送のみ)</summary>
        LiveStatus = 1 << 14,

        ///<summary>注意!! Undocumented Fields コミュ限 (生放送のみ？)</summary>
        MemberOnly = 1 << 15,

        ///<summary>注意！！Undocumented Fields 放送者種別 (生放送のみ？)</summary>
        ProviderType = 1 << 16,

        ///<summary>すべて</summary>
        All = (1 << 17) - 1,

        ///<summary>生放送で使えるもの</summary>
        LiveAll = All & ~MylistCounter & ~LastCommentTime & ~LengthSeconds & ~MemberOnly & ~ProviderType,
    }

    internal static class FieldsExtensions
    {
        ///<summary>Query用の文字列</summary>
        public static string ToQueryString(this Fields self)
        {
            if(self == 0) return "";

            var l = new List<string>();
            foreach(Fields f in Enum.GetValues(typeof(Fields)))
            {
                if(self.HasFlag(f))
                {
                    if(f == Fields.All) continue;
                    if(f == Fields.LiveAll) continue;

                    l.Add(f.ToLowerCamelCaseString());
                }
            }

            return string.Join(",", l);
        }
    }
}
