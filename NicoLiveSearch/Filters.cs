using System;
using System.Diagnostics;

namespace NicoLiveSearch
{
    ///<summary>検索フィルター条件 スタティックファクトリ</summary>
    public class Filters : IFilter
    {
        public CompOp Op { get; }
        public FiltersField Field { get; }
        public string Value { get; }

        // インスタンス化不可
        private Filters() { }
        private Filters(FiltersField field, string value, CompOp op = CompOp.EQ)
        {
            Debug.Assert(field.IsDefined());
            Field = field;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            if(!op.IsDefined()) throw new ArgumentOutOfRangeException(nameof(op));
            Op = op;
        }

        private Filters(FiltersField type, int value, CompOp op)
            : this(type, 0 <= value ? value.ToString() : throw new ArgumentOutOfRangeException(nameof(value)), op) { }
        private Filters(FiltersField type, bool value, CompOp op)
            : this(type, value ? "true" : "false", op) { }
        private Filters(FiltersField type, DateTime value, CompOp op)
            : this(type, value.ToString("yyyy-MM-ddThh:mm:ss"), op) { }


        /// <summary>放送者ID 検索フィルター生成</summary>
        /// <param name="value">該当する 放送者ID</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter UserId(int value)
            => new Filters(FiltersField.UserId, value, CompOp.EQ);

        /// <summary>チャンネルID 検索フィルター生成</summary>
        /// <param name="value">該当する チャンネルID</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter ChannelId(int value)
            => new Filters(FiltersField.ChannelId, value, CompOp.EQ);

        /// <summary>コミュニティID 検索フィルター生成</summary>
        /// <param name="value">該当する コミュニティID</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter CommunityId(int value)
            => new Filters(FiltersField.CommunityId, value, CompOp.EQ);

        /// <summary>放送元種別 検索フィルター生成</summary>
        /// <param name="value">該当する 放送元種別</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter ProviderType(ProviderType value)
        {
            if(!value.IsDefined()) throw new ArgumentOutOfRangeException(nameof(value));
            return new Filters(FiltersField.ProviderType, value.ToLowerCamelCaseString(), CompOp.EQ);
        }

        /// <summary>タグ 検索フィルター生成</summary>
        /// <param name="value">該当する タグ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter Tags(string value) => new Filters(FiltersField.Tags, value);

        /// <summary>完全一致タグ 検索フィルター生成</summary>
        /// <param name="value">該当する 完全一致タグ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter TagsExact(string value) => new Filters(FiltersField.TagsExact, value);

        /// <summary>来場者数 検索フィルター生成</summary>
        /// <param name="op">比較文字列(= > >= <= <)</param>
        /// <param name="value">該当する 来場者数</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter ViewCounter(string op, int value) => ViewCounter(value, GetOp(op));
        /// <summary>来場者数 検索フィルター生成</summary>
        /// <param name="value">該当する 来場者数</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter ViewCounter(int value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.ViewCounter, value, op);

        /// <summary>コメント数 検索フィルター生成</summary>
        /// <param name="op">比較文字列(= > >= <= <)</param>
        /// <param name="value">該当する コメント数</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter CommentCounter(string op, int value) => CommentCounter(value, GetOp(op));
        /// <summary>コメント数 検索フィルター生成</summary>
        /// <param name="value">該当する コメント数</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter CommentCounter(int value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.CommentCounter, value, op);

        /// <summary>開場日時 検索フィルター生成</summary>
        /// <param name="op">比較文字列(= > >= <= <)</param>
        /// <param name="value">該当する 開場日時</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter OpenTime(string op, DateTime value) => OpenTime(value, GetOp(op));
        /// <summary>開場日時 検索フィルター生成</summary>
        /// <param name="value">該当する 開場日時</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter OpenTime(DateTime value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.OpenTime, value, op);

        /// <summary>開始日時 検索フィルター生成</summary>
        /// <param name="op">比較文字列(= > >= <= <)</param>
        /// <param name="value">該当する 開始日時</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter StartTime(string op, DateTime value) => StartTime(value, GetOp(op));
        /// <summary>開始日時 検索フィルター生成</summary>
        /// <param name="value">該当する 開始日時</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter StartTime(DateTime value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.StartTime, value, op);

        /// <summary>タイムシフト視聴可能か 検索フィルター生成</summary>
        /// <param name="value">タイムシフト視聴可能か</param>
        /// <returns>検索フィルター条件</returns>
        public static IFilter TimeshiftEnabled(bool value)
            => new Filters(FiltersField.TimeshiftEnabled, value, CompOp.EQ);

        /// <summary>タイムシフト予約者数 検索フィルター生成</summary>
        /// <param name="op">比較文字列(= > >= <= <)</param>
        /// <param name="value">該当する タイムシフト予約者数</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter ScoreTimeshiftReserved(string op, int value)
            => ScoreTimeshiftReserved(value, GetOp(op));
        /// <summary>タイムシフト予約者数 検索フィルター生成</summary>
        /// <param name="value">該当する タイムシフト予約者数</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter ScoreTimeshiftReserved(int value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.ScoreTimeshiftReserved, value, op);

        /// <summary>コミュニティ名 検索フィルター生成</summary>
        /// <param name="value">該当する コミュニティ名</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter CommunityText(string value)
            => new Filters(FiltersField.CommunityText, value, CompOp.EQ);

        /// <summary>チャンネル・コミュニティ限定か 検索フィルター生成</summary>
        /// <param name="value">チャンネル・コミュニティ限定か</param>
        /// <returns>検索フィルター条件</returns>
        public static IFilter MemberOnly(bool value)
            => new Filters(FiltersField.MemberOnly, value, CompOp.EQ);

        /// <summary>放送ステータス 検索フィルター生成</summary>
        /// <param name="value">該当する 放送ステータス</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter LiveStatus(LiveStatus value)
        {
            if(!value.IsDefined()) throw new ArgumentOutOfRangeException(nameof(value));
            return new Filters(FiltersField.LiveStatus, value.ToLowerCamelCaseString());
        }

        private static CompOp GetOp(string op)
        {
            op = op ?? throw new ArgumentNullException(nameof(op));

            switch(op)
            {
                case "=": return CompOp.EQ;
                case ">": return CompOp.GT;
                case ">=": return CompOp.GTE;
                case "<=": return CompOp.LTE;
                case "<": return CompOp.LT;
                default: throw new ArgumentException(nameof(op));
            }
        }
    }
}
