using NNS_T.Utility;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NNS_T.Models
{
    ///<summary>ユーザー設定</summary>
    [XmlRoot("Settings")]
    public class SettingsModel : Observable
    {
        ///<summary>ウィンドウ設定</summary>
        public WindowModel Window { get; set; }

        ///<summary>通知設定</summary>
        public NotifyModel Notify { get; set; }

        ///<summary>検索設定</summary>
        public SearchModel Search { get; set; }

        ///<summary>ミュート設定</summary>
        public MuteModel Mute { get; set; }


        public SettingsModel() => Initialize();

        private void Initialize()
        {
            Window = new WindowModel();
            Notify = new NotifyModel();
            Search = new SearchModel();
            Mute = new MuteModel();
        }
        [OnDeserializing]
        private void OnDeserializing(StreamingContext sc) => Initialize();
    }
}
