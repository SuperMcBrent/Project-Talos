using Client.Models;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Views {
    /// <summary>
    /// Interaction logic for ProfileEditorView.xaml
    /// </summary>
    public partial class ProfileEditorView : Window {
        public ProfileEditorView() {
            InitializeComponent();
        }

        private ProfileEditorViewModel ViewModel {
            get { return DataContext as ProfileEditorViewModel; }
        }

        private void UntrackFile_Click(object sender, RoutedEventArgs e) {
            var menuItem = e.Source as MenuItem;
            if (menuItem != null) ViewModel.UntrackFileCommand.Execute(menuItem.DataContext as string);
        }

        //private void NewProfile_Click(object sender, RoutedEventArgs e) {
        //    var menuItem = e.Source as Button;
        //    if (menuItem != null) ViewModel.AddNewProfileCommand.Execute(null);
        //}

        private void DeleteProfile_Click(object sender, RoutedEventArgs e) {
            var menuItem = e.Source as Button;
            if (menuItem != null) ViewModel.DeleteProfileCommand.Execute(menuItem.DataContext as Profile);
        }

        private void NewProfile_Click(object sender, MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.AddNewProfileCommand.Execute(null);
            //var menuItem1 = e.Source as Image;
            //if (menuItem1 != null) ViewModel.AddNewProfileCommand.Execute(null);
        }

        private void SelectProfile_Click(object sender, MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.SelectProfileCommand.Execute(menuItem.DataContext as Profile);
        }

        private void AddTrackedFile_Click(object sender, MouseButtonEventArgs e) {
            var menuItem = e.Source as Border;
            if (menuItem != null) ViewModel.AddNewBotCommand.Execute(null);
        }

        private void ImportProfile_Click(object sender, RoutedEventArgs e) {
            var menuItem = e.Source as Button;
            if (menuItem != null) ViewModel.ImportProfileCommand.Execute(null);
        }

        private void DeleteAllProfiles_Click(object sender, RoutedEventArgs e) {
            var menuItem = e.Source as Button;
            if (menuItem != null) ViewModel.DeleteAllProfilesCommand.Execute(null);
        }

        private void ExportProfile_Click(object sender, RoutedEventArgs e) {
            var menuItem = e.Source as Button;
            if (menuItem != null) ViewModel.ExportProfileCommand.Execute(menuItem.DataContext as Profile);
        }

        private void SelectActiveProfile_Click(object sender, RoutedEventArgs e) {
            Debug.WriteLine($"Setting active profile.");
            var textBlock = e.Source as Button;
            if (textBlock != null) ViewModel.SetActiveProfileCommand.Execute(textBlock.DataContext as Profile);
        }

        private void DiscardAllProfileChanges_Click(object sender, RoutedEventArgs e) {
            Debug.WriteLine($"Discarding all changes.");
            ViewModel.DiscardAllProfileChangesCommand.Execute(this);
        }
    }
}
