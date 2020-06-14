using System;
using System.Collections.Generic;
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
    /// Interaction logic for DiscardAllProfileChangesDialog.xaml
    /// </summary>
    public partial class DiscardAllProfileChangesDialog : Window {
        public DiscardAllProfileChangesDialog() {
            InitializeComponent();
        }
        public bool Response { get; private set; }

        /// <summary>
        /// You don't want to drop your profiles.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnNo_Clicked(object sender, RoutedEventArgs e) {
            Response = false;
            Close();
        }

        /// <summary>
        /// You want to drop your profiles.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnYes_Clicked(object sender, RoutedEventArgs e) {
            Response = true;
            Close();
        }
    }
}
