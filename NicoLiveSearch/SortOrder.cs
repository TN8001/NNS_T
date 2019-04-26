using System;

namespace NicoLiveSearch
{
    ///<summary>ソート順</summary>
    public class SortOrder
    {
        ///<summary>レスポンスデータ</summary>
        public SortField Field { get; }

        ///<summary>昇順 降順 (falseで大きい 新しいものが上)</summary>
        public bool Ascending { get; }


        /// <summary>放送者ID 昇順</summary>
        public static SortOrder UserIdAsc => userIdAsc ?? (userIdAsc = new SortOrder(SortField.UserId, true));
        private static SortOrder userIdAsc;
        /// <summary>放送者ID 降順</summary>
        public static SortOrder UserIdDesc => userIdDesc ?? (userIdDesc = new SortOrder(SortField.UserId));
        private static SortOrder userIdDesc;

        /// <summary>チャンネルID 昇順</summary>
        public static SortOrder ChannelIdAsc => channelIdAsc ?? (channelIdAsc = new SortOrder(SortField.ChannelId, true));
        private static SortOrder channelIdAsc;
        /// <summary>チャンネルID 降順</summary>
        public static SortOrder ChannelIdDesc => channelIdDesc ?? (channelIdDesc = new SortOrder(SortField.ChannelId));
        private static SortOrder channelIdDesc;

        /// <summary>コミュニティID 昇順</summary>
        public static SortOrder CommunityIdAsc => communityIdAsc ?? (communityIdAsc = new SortOrder(SortField.CommunityId, true));
        private static SortOrder communityIdAsc;
        /// <summary>コミュニティID 降順</summary>
        public static SortOrder CommunityIdDesc => communityIdDesc ?? (communityIdDesc = new SortOrder(SortField.CommunityId));
        private static SortOrder communityIdDesc;

        /// <summary>来場者数 昇順</summary>
        public static SortOrder ViewCounterAsc => viewCounterAsc ?? (viewCounterAsc = new SortOrder(SortField.ViewCounter, true));
        private static SortOrder viewCounterAsc;
        /// <summary>来場者数 降順</summary>
        public static SortOrder ViewCounterDesc => viewCounterDesc ?? (viewCounterDesc = new SortOrder(SortField.ViewCounter));
        private static SortOrder viewCounterDesc;

        /// <summary>コメント数 昇順</summary>
        public static SortOrder CommentCounterAsc => commentCounterAsc ?? (commentCounterAsc = new SortOrder(SortField.CommentCounter, true));
        private static SortOrder commentCounterAsc;
        /// <summary>コメント数 降順</summary>
        public static SortOrder CommentCounterDesc => commentCounterDesc ?? (commentCounterDesc = new SortOrder(SortField.CommentCounter));
        private static SortOrder commentCounterDesc;

        /// <summary>開場日時 昇順</summary>
        public static SortOrder OpenTimeAsc => openTimeAsc ?? (openTimeAsc = new SortOrder(SortField.OpenTime, true));
        private static SortOrder openTimeAsc;
        /// <summary>開場日時 降順</summary>
        public static SortOrder OpenTimeDesc => openTimeDesc ?? (openTimeDesc = new SortOrder(SortField.OpenTime));
        private static SortOrder openTimeDesc;

        /// <summary>開始日時 昇順</summary>
        public static SortOrder StartTimeAsc => startTimeAsc ?? (startTimeAsc = new SortOrder(SortField.StartTime, true));
        private static SortOrder startTimeAsc;
        /// <summary>開始日時 降順</summary>
        public static SortOrder StartTimeDesc => startTimeDesc ?? (startTimeDesc = new SortOrder(SortField.StartTime));
        private static SortOrder startTimeDesc;

        /// <summary>タイムシフト予約者数 昇順</summary>
        public static SortOrder ScoreTimeshiftReservedAsc => scoreTimeshiftReservedAsc ?? (scoreTimeshiftReservedAsc = new SortOrder(SortField.ScoreTimeshiftReserved, true));
        private static SortOrder scoreTimeshiftReservedAsc;
        /// <summary>タイムシフト予約者数 降順</summary>
        public static SortOrder ScoreTimeshiftReservedDesc => scoreTimeshiftReservedDesc ?? (scoreTimeshiftReservedDesc = new SortOrder(SortField.ScoreTimeshiftReserved));
        private static SortOrder scoreTimeshiftReservedDesc;


        /// <summary>ソート順</summary>
        /// <param name="field">ソートのキーにするフィールド</param>
        /// <param name="ascending">昇順 降順 (falseで大きい 新しいものが上)</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public SortOrder(SortField field, bool ascending = false)
        {
            if(!field.IsDefined()) throw new ArgumentOutOfRangeException(nameof(field));
            (Field, Ascending) = (field, ascending);
        }

        ///<summary>クエリ文字列</summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception> 
        public string ToQueryString() => (Ascending ? "+" : "-") + Field.ToLowerCamelCaseString();
    }
}
