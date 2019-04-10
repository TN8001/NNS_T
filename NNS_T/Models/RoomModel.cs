using System;
using System.Xml.Serialization;
using NNS_T.Models.NicoAPI;

namespace NNS_T.Models
{
    // ミュート用
    ///<summary>部屋の情報</summary>
    public class RoomModel : IEquatable<RoomModel>
    {
        ///<summary>部屋のID (co1234567等)</summary>
        [XmlAttribute]
        public string ID { get; set; }

        ///<summary>部屋の名前</summary>
        [XmlAttribute]
        public string Title { get; set; }

        ///<summary>部屋アイコンのURL</summary>
        [XmlAttribute]
        public string IconUrl { get; set; }

        ///<summary>部屋のURL</summary>
        public string RoomUrl => ID.StartsWith("co") ? NicoApi.CommunityUrl + ID
                                                     : NicoApi.ChannelUrl + ID;

        ///<summary>部屋の種類</summary>
        public ProviderType ProviderType => ID.StartsWith("co") ? ProviderType.User
                                                                : ProviderType.Channel;


        public RoomModel() { } // XmlSerializer用
        public RoomModel(string id, string title, string iconUrl)
        {
            ID = id;
            Title = title;
            IconUrl = iconUrl;
        }

        public bool Equals(RoomModel other) => other == null ? false : (ID == other.ID);
        public override bool Equals(object obj) => (obj == null || GetType() != obj.GetType()) ? false : Equals((RoomModel)obj);
        public override int GetHashCode() => ID.GetHashCode();
    }
}
