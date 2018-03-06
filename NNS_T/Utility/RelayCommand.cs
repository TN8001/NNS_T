// Copyright (c) 2010 @okazuki
// http://blog.okazuki.jp/entry/20100223/1266897125

// 改変 (c) 2017 T.Naga
// クラス名変更 最新の書き方に更新

using System;
using System.Windows.Input;

namespace NNS_T.Utility
{
    #region No parameter RelayCommand
    ///<summary>DelegateCommand</summary>
    public sealed class RelayCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

        public RelayCommand(Action execute) : this(execute, () => true) { }
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute() => _canExecute();
        public void Execute() => _execute();

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        bool ICommand.CanExecute(object parameter) => CanExecute();
        void ICommand.Execute(object parameter) => Execute();
    }
    #endregion

    #region Parameter RelayCommand
    ///<summary>DelegateCommand</summary>
    public sealed class RelayCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Func<T, bool> _canExecute;
        private static readonly bool IS_VALUE_TYPE;

        static RelayCommand() => IS_VALUE_TYPE = typeof(T).IsValueType;

        public RelayCommand(Action<T> execute) : this(execute, o => true) { }
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(T parameter) => _canExecute(parameter);
        public void Execute(T parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        bool ICommand.CanExecute(object parameter) => CanExecute(Cast(parameter));
        void ICommand.Execute(object parameter) => Execute(Cast(parameter));

        private T Cast(object parameter)
        {
            if(parameter == null && IS_VALUE_TYPE) return default(T);
            if(parameter is T) return (T)parameter;
            else return default(T);
        }
    }
    #endregion
}
