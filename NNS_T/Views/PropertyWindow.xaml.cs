using System.Windows;

namespace NNS_T.Views
{
    public partial class PropertyWindow : Window
    {
        public PropertyWindow(object o)
        {
            InitializeComponent();
            DataContext = o;
        }
    }
}
