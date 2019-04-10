using System;

namespace NNS_T.Models.NicoAPI
{
    ///<summary>検索フィルター条件</summary>
    public interface IFilter
    {
        ///<summary>比較方法</summary>
        CompOp Op { get; }
        ///<summary>対象フィールド</summary>
        FiltersField Field { get; }
        ///<summary>比較する値の文字列表現</summary>
        string Value { get; }
    }


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
            //if(!Enum.IsDefined(field.GetType(), field)) throw new ArgumentOutOfRangeException(nameof(field));
            if(value == null) throw new ArgumentNullException(nameof(value));
            if(!Enum.IsDefined(op.GetType(), op)) throw new ArgumentOutOfRangeException(nameof(op));

            (Op, Field, Value) = (op, field, value);
        }

        private Filters(FiltersField type, int value, CompOp op) : this(type, value.ToString(), op) { }
        private Filters(FiltersField type, DateTime value, CompOp op)
            : this(type, value.ToString("yyyy-MM-ddThh:mm:ss"), op) { }


        /// <summary>タグ 検索フィルター生成</summary>
        /// <param name="value">該当する タグ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter Tags(string value) => new Filters(FiltersField.Tags, value);

        /// <summary>カテゴリタグ 検索フィルター生成</summary>
        /// <param name="value">該当する カテゴリタグ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter CategoryTags(string value) => new Filters(FiltersField.CategoryTags, value);

        /// <summary>再生数 検索フィルター生成</summary>
        /// <param name="op">比較文字列（= > >= <= <）</param>
        /// <param name="value">該当する 再生数</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter ViewCounter(string op, int value) => ViewCounter(value, GetOp(op));
        /// <summary>再生数 検索フィルター生成</summary>
        /// <param name="value">該当する 再生数</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter ViewCounter(int value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.ViewCounter, value, op);

        /// <summary>マイリスト数 検索フィルター生成</summary>
        /// <param name="op">比較文字列（= > >= <= <）</param>
        /// <param name="value">該当する マイリスト数</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter MylistCounter(string op, int value) => MylistCounter(value, GetOp(op));
        /// <summary>マイリスト数 検索フィルター生成</summary>
        /// <param name="value">該当する マイリスト数</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter MylistCounter(int value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.MylistCounter, value, op);

        /// <summary>コメント数 検索フィルター生成</summary>
        /// <param name="op">比較文字列（= > >= <= <）</param>
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

        /// <summary>投稿日時 検索フィルター生成</summary>
        /// <param name="op">比較文字列（= > >= <= <）</param>
        /// <param name="value">該当する 投稿日時</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter StartTime(string op, DateTime value) => StartTime(value, GetOp(op));
        /// <summary>投稿日時 検索フィルター生成</summary>
        /// <param name="value">該当する 投稿日時</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter StartTime(DateTime value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.StartTime, value, op);

        /// <summary>最新コメント日時 検索フィルター生成</summary>
        /// <param name="op">比較文字列（= > >= <= <）</param>
        /// <param name="value">該当する 最新コメント日時</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter LastCommentTime(string op, DateTime value) => LastCommentTime(value, GetOp(op));
        /// <summary>最新コメント日時 検索フィルター生成</summary>
        /// <param name="value">該当する 最新コメント日時</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter LastCommentTime(DateTime value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.LastCommentTime, value, op);

        /// <summary>再生時間 検索フィルター生成</summary>
        /// <param name="op">比較文字列（= > >= <= <）</param>
        /// <param name="value">該当する 再生時間</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter LengthSeconds(string op, int value) => LengthSeconds(value, GetOp(op));
        /// <summary>再生時間 検索フィルター生成</summary>
        /// <param name="value">該当する 再生時間</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter LengthSeconds(int value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.LengthSeconds, value, op);

        /// <summary>タイムシフト予約者数 検索フィルター生成</summary>
        /// <param name="op">比較文字列（= > >= <= <）</param>
        /// <param name="value">該当する タイムシフト予約者数</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentException"></exception> 
        /// <exception cref="ArgumentNullException"></exception> 
        public static IFilter ScoreTimeshiftReserved(string op, int value) => ScoreTimeshiftReserved(value, GetOp(op));
        /// <summary>タイムシフト予約者数 検索フィルター生成</summary>
        /// <param name="value">該当する タイムシフト予約者数</param>
        /// <param name="op">比較オペレータ</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter ScoreTimeshiftReserved(int value, CompOp op = CompOp.EQ)
            => new Filters(FiltersField.ScoreTimeshiftReserved, value, op);

        /// <summary>放送ステータス 検索フィルター生成</summary>
        /// <param name="value">該当する 放送ステータス</param>
        /// <returns>検索フィルター条件</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public static IFilter LiveStatus(LiveStatus value)
            => new Filters(FiltersField.LiveStatus, value.ToLowerCamelCaseString());


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
