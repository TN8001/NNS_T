using NNS_T.Utility;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NNS_T.Models
{
    ///<summary>ミュートしている部屋のコレクション</summary>
    public class MuteCollection : ObservableCollection<RoomModel> { }

    ///<summary>ミュート設定</summary>
    public class MuteModel : Observable
    {
        ///<summary>公式放送をミュート</summary>
        [XmlAttribute]
        public bool OfficialIgnored { get => _OfficialIgnored; set => Set(ref _OfficialIgnored, value); }
        private bool _OfficialIgnored;

        ///<summary>ミュートを一覧に非表示</summary>
        [XmlAttribute]
        public bool HideList { get => _HideList; set => Set(ref _HideList, value); }
        private bool _HideList;

        ///<summary>ミュートしている部屋</summary>
        [XmlArrayItem("Item")]
        public MuteCollection Items { get; set; }


        public MuteModel() => Initialize();

        private void Initialize()
        {
            OfficialIgnored = false;
            HideList = true;
            Items = new MuteCollection();
        }
        [OnDeserializing]
        private void OnDeserializing(StreamingContext sc) => Initialize();
    }
}
