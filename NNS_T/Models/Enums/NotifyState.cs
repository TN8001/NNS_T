
namespace NNS_T.Models
{
    /// <summary>通知を出す状態</summary>
    public enum NotifyState
    {
        /// <summary>常に出す</summary>
        Always,

        /// <summary>非アクティブ時に出す</summary>
        Inactive,

        /// <summary>アイコン化時に出す</summary>
        Minimize,

        /// <summary>常に出さない</summary>
        Never,
    }
}
