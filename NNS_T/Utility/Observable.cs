using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NNS_T.Utility
{
    // 直前にuwpを調べていたのでuwpっぽい命名にｗ 
    public abstract class Observable : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        // SetはSetPropertyより短くて断然良いですね
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if(Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
