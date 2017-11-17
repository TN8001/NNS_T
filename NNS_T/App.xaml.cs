using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NNS_T
{
    public partial class App : Application
    {
        public App() => InitializeComponent();

        public void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
            => ((Image)sender).Source = Current.Resources["NoImageImage"] as DrawingImage;
    }
}
