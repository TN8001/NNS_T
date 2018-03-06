using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NNS_T.Utility
{
    ///<summary>ViewModelBase</summary>
    public abstract class Observable : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        ///<summary>SetProperty</summary>
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
