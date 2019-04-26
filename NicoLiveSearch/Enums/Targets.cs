using System;

namespace NicoLiveSearch
{
    // Tags, TagsExact は排他指定(重複すると単にTags扱い)
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

        ///<summary>タグ検索(キーワードに完全一致するタグがあるコンテンツ)</summary>
        TagsExact = 8,

        // Allがあると ToLowerCamelCaseStringが使えなくなってしまう...
        /////<summary>すべて</summary>
        //All = Title | Description | Tags | TagsExact,
    }

    internal static class TargetsExtensions
    {
        ///<summary>Query用の文字列</summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static string ToQueryString(this Targets self) => self.ToLowerCamelCaseString();
    }
}
