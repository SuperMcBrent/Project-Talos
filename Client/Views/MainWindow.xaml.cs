using Client.Models;
using Client.ViewModels;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private void AddBot_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.AddBotCommand.Execute(null);
        }

        private void ConnectBot_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.ConnectCommand.Execute(menuItem.DataContext as Bot);
        }

        private void DisconnectBot_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.DisconnectCommand.Execute(menuItem.DataContext as Bot);
        }

        private void RegisterBot_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.RegisterCommand.Execute(menuItem.DataContext as Bot);
        }
    }
}