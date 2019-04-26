using System;
using System.Xml.Serialization;
using NicoLiveSearch;

namespace NNS_T.Models
{
    // ミュート用
    ///<summary>部屋の情報</summary>
    public class RoomModel : IEquatable<RoomModel>
    {
        ///<summary>部屋のID (co1234567等)</summary>
        [XmlAttribute("ID")]
        public string Id { get; set; }

        ///<summary>部屋の名前</summary>
        [XmlAttribute]
        public string Title { get; set; }

        ///<summary>部屋アイコンのURL</summary>
        [XmlAttribute]
        public string IconUrl { get; set; }

        ///<summary>部屋のURL</summary>
        public string RoomUrl => Id.StartsWith("co") ? NicoLiveApi.CommunityUrl + Id
                                                     : NicoLiveApi.ChannelUrl + Id;

        ///<summary>部屋の種類</summary>
        public ProviderType ProviderType => Id.StartsWith("co") ? ProviderType.Community
                                                                : ProviderType.Channel;

        ///<summary>放送者種別表示文字列</summary>
        public string ProviderTypeString => ProviderType == ProviderType.Community ? "User" : ProviderType.ToString();



        public RoomModel() { } // XmlSerializer用
        public RoomModel(string id, string title, string iconUrl)
        {
            Id = id;
            Title = title;
            IconUrl = iconUrl;
        }

        public bool Equals(RoomModel other) => other == null ? false : (Id == other.Id);
        public override bool Equals(object obj) => (obj == null || GetType() != obj.GetType()) ? false : Equals((RoomModel)obj);
        public override int GetHashCode() => Id.GetHashCode();
    }
}
