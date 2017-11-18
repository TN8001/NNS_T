using NNS_T.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NNS_T.Views
{
    public partial class SearchPane : UserControl
    {
        public SearchPane() => InitializeComponent();

        // TextBoxのbindingを走らせてからコマンド実行
        private void Button_Click(object sender, RoutedEventArgs e) => ((Button)sender).Focus();

        // 裏コマンド 取得タイマOnOff HitCountを左ボタン押したまま右クリック
        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                ((MainViewModel)DataContext).ToggleTimerCommand.Execute();
        }
    }
}
