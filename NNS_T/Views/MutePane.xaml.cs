using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NNS_T.Views
{
    public partial class MutePane : UserControl
    {
        public MutePane() => InitializeComponent();

        public void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
            => ((Image)sender).Source = Application.Current.Resources["NoImageImage"] as DrawingImage;
    }
}
