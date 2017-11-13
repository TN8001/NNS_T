using NNS_T.Utility;
using NNS_T.Views;
using System.ComponentModel;
using System.Runtime.Serialization;
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

        ///<summary>通知onoff</summary> // デバッグ用
        [XmlAttribute, DefaultValue(true)]
        public bool IsEnabled { get => _IsEnabled; set { if(Set(ref _IsEnabled, value)) if(!value) ToastWindow.Clear(); } }
        private bool _IsEnabled;

        ///<summary>通知表示時間</summary>
        [XmlAttribute]
        public int ShowSec { get => ToastWindow.ShowSec; set { ToastWindow.ShowSec = value; OnPropertyChanged(); } }


        public NotifyModel() => Initialize();

        private void Initialize()
        {
            Sound = NotifySound.System;
            State = NotifyState.Always;
            IsEnabled = true;
            ShowSec = 15;
        }
        [OnDeserializing]
        private void OnDeserializing(StreamingContext sc) => Initialize();
    }
}
