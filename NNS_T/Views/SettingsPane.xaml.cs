using NNS_T.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace NNS_T.Views
{
    ///<summary>enumと表示名の対応付け用 XAMLで指定できるようにするため専用クラスに</summary>
    public class NotifySoundDictionary : Dictionary<NotifySound, string> { }

    ///<summary>enumと表示名の対応付け用 XAMLで指定できるようにするため専用クラスに</summary>
    public class NotifyPatternDictionary : Dictionary<NotifyState, string> { }

    public partial class SettingsPane : UserControl
    {
        public SettingsPane() => InitializeComponent();
    }
}
