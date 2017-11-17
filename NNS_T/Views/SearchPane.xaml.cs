using NNS_T.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NNS_T.Views
{
    public partial class SearchPane : UserControl
    {
        public SearchPane() => InitializeComponent();

        //public void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        //    => ((Image)sender).Source = Application.Current.Resources["NoImageImage"] as DrawingImage;

        void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            => new PropertyWindow(((ListBoxItem)sender).DataContext).Show();

        // TextBoxのbindingを走らせてからコマンド実行
        private void Button_Click(object sender, RoutedEventArgs e) => ((Button)sender).Focus();

        // 裏コマンド
        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                ((MainViewModel)DataContext).ToggleTimerCommand.Execute();
        }
    }
}
