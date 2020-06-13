using Client.Business;
using Client.ViewModels;
using Unity;

namespace Client.Framework {

    /// <summary>
    /// Here the DI magic come on.
    /// </summary>
    public class Bootstrapper {
        public IUnityContainer Container { get; set; }

        public Bootstrapper() {
            Container = new UnityContainer();

            ConfigureContainer();
        }

        /// <summary>
        /// We register here every service / interface / viewmodel.
        /// </summary>
        private void ConfigureContainer() {
            Container.RegisterInstance<IProfileRepository>(new ProfileRepository("Profiles"));
            Container.RegisterInstance<IBotRepository>(new BotRepository("Bots")); // folder met files
            Container.RegisterType<MainViewModel>();
            Container.RegisterType<ProfileEditorViewModel>();
        }
    }
}