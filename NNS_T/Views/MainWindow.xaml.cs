using MahApps.Metro.Controls;
using NNS_T.ViewModels;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace NNS_T.Views
{
    public partial class MainWindow //: Window
    {
        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();

            var l = (IList)HamburgerMenuControl.ItemsSource;
            var h = (HamburgerMenuItem)l[0];
            ((FrameworkElement)h.Tag).DataContext = DataContext;
            HamburgerMenuControl.Content = h;
        }

        private void HamburgerMenuControl_OnItemClick(object sender, ItemClickEventArgs e)
        {
            HamburgerMenuControl.IsPaneOpen = false;
            var h = (HamburgerMenuItem)e.ClickedItem;
            ((FrameworkElement)h.Tag).DataContext = DataContext;
            HamburgerMenuControl.Content = h;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            ToastWindow.Clear();
            ((MainViewModel)DataContext).SaveCommand.Execute();
        }
    }
}
