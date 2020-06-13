using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        private MainViewModel ViewModel {
            get { return DataContext as MainViewModel; }
        }

        private void OpenChangeProfileWindow_Click(object sender, RoutedEventArgs e) {
            var menuItem = e.Source as MenuItem;
            if (menuItem != null) ViewModel.OpenChangeProfileWindow.Execute(menuItem.DataContext);
        }
    }
}