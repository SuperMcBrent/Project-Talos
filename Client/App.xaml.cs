using GalaSoft.MvvmLight.Threading;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace Client {

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private static void InitializeCultures(CultureInfo cultureInfo) {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        static App() {
            InitializeCultures(new CultureInfo("en-us"));
            DispatcherHelper.Initialize();
        }
    }
}