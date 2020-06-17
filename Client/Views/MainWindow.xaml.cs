using Client.Models;
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
            if (menuItem != null) ViewModel.OpenChangeProfileWindowCommand.Execute(menuItem.DataContext);
        }

        private void CollapseBotList_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.CollapseBotListCommand.Execute(null);
        }

        private void SelectBot_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.SelectBotCommand.Execute(menuItem.DataContext as Bot);
        }

        private void DefaultSize_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var menuItem = e.Source as TextBlock;
            if (menuItem != null) ViewModel.DefaultSizeCommand.Execute(null);
        }
    }
}