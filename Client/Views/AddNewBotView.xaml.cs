using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views {

    /// <summary>
    /// Interaction logic for AddBotView.xaml
    /// </summary>
    public partial class AddNewBotView : Window {

        public AddNewBotView() {
            InitializeComponent();
        }

        private AddNewBotViewModel ViewModel {
            get { return DataContext as AddNewBotViewModel; }
        }

        /// <summary>
        /// You don't want to drop your profiles.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnClose_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// You want to drop your profiles.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddButton_Click(object sender, RoutedEventArgs e) {
            var menuItem = e.Source as Button;
            if (menuItem != null) ViewModel.AddBotCommand.Execute(null);
            Close();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e) {
            var menuItem = e.Source as Button;
            if (menuItem != null) ViewModel.BrowseCommand.Execute(null);
        }
    }
}