using System;

namespace NNS_T.Models.NicoAPI
{
    ///<summary>[Flags] 検索対象フラグ</summary>
    [Flags]
    public enum Targets
    {
        ///<summary>キーワード検索 タイトル</summary>
        Title = 1,

        ///<summary>キーワード検索 説明文</summary>
        Description = 2,

        ///<summary>キーワード検索 タグ</summary>
        Tags = 4,

        ///<summary>タグ検索（キーワードに完全一致するタグがあるコンテンツ）</summary>
        TagsExact = 8,

        ///<summary>すべて</summary>
        All = Title | Description | Tags | TagsExact,
    }

    internal static class TargetsExtensions
    {
        ///<summary>Query用の文字列</summary>
        public static string ToQueryString(this Targets self)
        {
            if(self == 0) return "";

            return self.HasFlag(Targets.TagsExact)
                ? "tagsExact"
                : self.ToString().ToLower().Replace(" ", "");
        }
    }
}
