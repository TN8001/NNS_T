using NNS_T.Utility;
using NNS_T.Views;
using System.Xml.Serialization;

namespace NNS_T.Models
{
    ///<summary>通知設定</summary>
    public class NotifyModel : Observable
    {
        ///<summary>通知音</summary>
        [XmlAttribute]
        public NotifySound Sound { get => ToastWindow.NotifySound; set => Set(ref ToastWindow.NotifySound, value); }

        ///<summary>通知を出す状態</summary>
        [XmlAttribute]
        public NotifyState State { get => _State; set => Set(ref _State, value); }
        private NotifyState _State;

        ///<summary>通知表示時間（秒）</summary>
        [XmlAttribute]
        public int ShowSec { get => ToastWindow.ShowSec; set { ToastWindow.ShowSec = value; OnPropertyChanged(); } }


        public NotifyModel()
        {
            Sound = NotifySound.System;
            State = NotifyState.Always;
            ShowSec = 15;
        }
    }
}
