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
        public bool Official { get => _Official; set => Set(ref _Official, value); }
        private bool _Official;

        ///<summary>ミュートを一覧に表示</summary>
        [XmlAttribute]
        public bool ShowList { get => _showList; set { if(Set(ref _showList, value)) OnPropertyChanged(nameof(HideList)); } }
        private bool _showList;
        public bool HideList { get => !_showList;  }

        ///<summary>ミュートしている部屋</summary>
        [XmlArrayItem("Item")]
        public MuteCollection Items { get; set; }


        public MuteModel() => Initialize();

        private void Initialize()
        {
            _Official = false;
            _showList = false;
            Items = new MuteCollection();
        }
        [OnDeserializing]
        private void OnDeserializing(StreamingContext sc) => Initialize();
    }
}
