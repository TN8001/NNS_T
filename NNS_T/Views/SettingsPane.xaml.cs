using NNS_T.Models;
using NNS_T.Utility;
using System.Collections.Generic;
using System.Windows.Controls;

namespace NNS_T.Views
{
    ///<summary>NotifySoundと表示名の対応付け用辞書 XAMLで指定できるようにするため専用クラス</summary>
    public class NotifySoundDictionary : Dictionary<NotifySound, string> { }

    ///<summary>NotifyStateと表示名の対応付け用辞書 XAMLで指定できるようにするため専用クラス</summary>
    public class NotifyPatternDictionary : Dictionary<NotifyState, string> { }

    ///<summary>ThemeStateと表示名の対応付け用辞書 XAMLで指定できるようにするため専用クラス</summary>
    public class WindowThemeDictionary : Dictionary<ThemeState, string> { }

    public partial class SettingsPane : UserControl
    {
        public SettingsPane() => InitializeComponent();
    }
}
