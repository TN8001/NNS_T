using MahApps.Metro;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NNS_T
{
    public partial class App : Application
    {
        public App() => InitializeComponent();

        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.AddAccent("LightThemeBlue", new Uri("pack://application:,,,/NNS_T;component/Themes/LightThemeBlue.xaml"));
            ThemeManager.AddAccent("DarkThemeBlue", new Uri("pack://application:,,,/NNS_T;component/Themes/DarkThemeBlue.xaml"));

            base.OnStartup(e);
        }

        public void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
            => ((Image)sender).Source = Current.Resources["NoImageImage"] as DrawingImage;
    }
}
