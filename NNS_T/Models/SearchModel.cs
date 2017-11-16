using NNS_T.Models.NicoAPI;
using NNS_T.Utility;
using System.Runtime.Serialization;
using System.Xml.Serialization;

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

        ///<summary>検索間隔</summary>
        [XmlAttribute]
        public int IntervalSec { get => _IntervalSec; set => Set(ref _IntervalSec, value); }
        private int _IntervalSec;

        ///<summary>レスポンスに使用するフィールド</summary>
        [XmlIgnore]
        public Fields Fields;

        ///<summary>レスポンスに説明文を含めるかどうか</summary>
        [XmlAttribute]
        public bool UnuseDescription
        {
            get => !Fields.HasFlag(Fields.Description);
            set
            {
                if(UnuseDescription == value) return;
                Fields = value ? Fields & ~Fields.Description
                               : Fields | Fields.Description;
                OnPropertyChanged();
            }
        }


        public SearchModel() => Initialize();

        private void Initialize()
        {
            Targets = Targets.Title | Targets.Description | Targets.Tags;
            IntervalSec = 30;
            Fields = Fields.LiveAll;
        }
        [OnDeserializing]
        private void OnDeserializing(StreamingContext sc) => Initialize();
    }
}
