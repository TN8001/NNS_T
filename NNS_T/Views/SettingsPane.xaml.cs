using NNS_T.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace NNS_T.Views
{
    ///<summary>NotifySoundと表示名の対応付け用辞書 XAMLで指定できるようにするため専用クラス</summary>
    public class NotifySoundDictionary : Dictionary<NotifySound, string> { }

    ///<summary>NotifyStateと表示名の対応付け用辞書 XAMLで指定できるようにするため専用クラス</summary>
    public class NotifyPatternDictionary : Dictionary<NotifyState, string> { }

    public partial class SettingsPane : UserControl
    {
        public SettingsPane() => InitializeComponent();
    }
}
