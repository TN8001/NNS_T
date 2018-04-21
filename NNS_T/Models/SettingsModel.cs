using NNS_T.Utility;
using NNS_T.Views;
using System.Xml.Serialization;

namespace NNS_T.Models
{
    ///<summary>ユーザー設定</summary>
    [XmlRoot("Settings")]
    public class SettingsModel : Observable
    {
        //TODO 設定ファイルのバージョン管理
        //大きく変更がかかる場合シリアライズしてからでは遅い
        //ファイル先頭のコメントを1行読んでから振り分ける等が必要
        //現状大変更もなさそうなのでboolを追加していくような小手先でやっていくしかないか？。。
        //[XmlAttribute("Version")]
        //public string _Version;
        //[XmlIgnore]
        //public Version Version
        //{
        //    get
        //    {
        //        if(string.IsNullOrEmpty(_Version))
        //            return new Version("0.0.0");
        //        return new Version(_Version);
        //    }

        //    set => _Version = value.ToString();
        //}


        ///<summary>ウィンドウ設定</summary>
        public WindowModel Window { get; set; }

        ///<summary>通知設定</summary>
        public NotifyModel Notify { get; set; }

        ///<summary>検索設定</summary>
        public SearchModel Search { get; set; }

        ///<summary>ミュート設定</summary>
        public MuteModel Mute { get; set; }

        ///<summary>開くブラウザのパス</summary>
        public string BrowserPath { get => HyperlinkEx.BrowserPath; set => Set(ref HyperlinkEx.BrowserPath, value); }

        ///<summary>新しいバージョンがリリースされているかを確認する
        ///nullはこの機能を付ける以前の設定ファイルとし確認ダイアログを出す</summary>
        public bool? UpdateCheck { get => _UpdateCheck; set => Set(ref _UpdateCheck, value); }
        private bool? _UpdateCheck;
        [XmlIgnore] public bool UpdateCheckSpecified => UpdateCheck != null;

        public SettingsModel()
        {
            Window = new WindowModel();
            Notify = new NotifyModel();
            Search = new SearchModel();
            Mute = new MuteModel();
        }
    }
}
