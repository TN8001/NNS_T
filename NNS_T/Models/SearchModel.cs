using System.Xml.Serialization;
using NicoLiveSearch;
using NNS_T.Utility;

namespace NNS_T.Models
{
    ///<summary>検索設定</summary>
    public class SearchModel : Observable
    {
        ///<summary>検索キーワード</summary>
        public string Query { get => _Query; set => Set(ref _Query, value); }
        private string _Query;

        ///<summary>検索方法</summary>
        [XmlAttribute]
        public Targets Targets { get => _Targets; set => Set(ref _Targets, value); }
        private Targets _Targets;

        ///<summary>検索間隔(秒)</summary>
        [XmlAttribute]
        public int IntervalSec
        {
            get => _IntervalSec;
            set
            {
                if(value < 10) value = 10;
                Set(ref _IntervalSec, value);
            }
        }
        private int _IntervalSec;

        ///<summary>レスポンスに使用するフィールド</summary>
        [XmlIgnore]
        public ResponseFields Fields;

        // 否定形はいまいちだが面倒なのでxamlでの使い勝手を優先ｗ
        ///<summary>レスポンスに説明文を含めないかどうか</summary>
        [XmlAttribute]
        public bool UnuseDescription
        {
            get => !Fields.HasFlag(ResponseFields.Description);
            set
            {
                if(UnuseDescription == value) return;
                Fields = value ? Fields & ~ResponseFields.Description
                               : Fields | ResponseFields.Description;
                OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public bool HideMemberOnly { get => _HideMemberOnly; set => Set(ref _HideMemberOnly, value); }
        private bool _HideMemberOnly;



        public SearchModel()
        {
            Targets = Targets.Title | Targets.Description | Targets.Tags;
            IntervalSec = 30;

            // UserId OpenTime LiveStatus 以外
            Fields = ResponseFields.ContentId | ResponseFields.Title | ResponseFields.Description
                   | ResponseFields.ChannelId | ResponseFields.CommunityId | ResponseFields.ProviderType
                   | ResponseFields.Tags | ResponseFields.ViewCounter | ResponseFields.CommentCounter
                   | ResponseFields.StartTime | ResponseFields.TimeshiftEnabled | ResponseFields.ScoreTimeshiftReserved
                   | ResponseFields.ThumbnailUrl | ResponseFields.CommunityText | ResponseFields.CommunityIcon
                   | ResponseFields.MemberOnly;
        }
    }
}
