using NNS_T.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace NNS_T.Models
{
    ///<summary>ミュートしている部屋のコレクション</summary>
    public class MuteCollection : ObservableCollection<RoomModel>
    {
        public MuteCollection() : base() { }
        public MuteCollection(IEnumerable<RoomModel> collection) : base(collection) { }
    }

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
        public bool HideList => !_showList;

        ///<summary>ミュートしている部屋</summary>
        [XmlArrayItem("Item")]
        public MuteCollection Items { get; set; } = new MuteCollection();
    }
}
