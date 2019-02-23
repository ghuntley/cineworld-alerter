using Unity;

namespace CineworldAlerter.ViewModels
{
    public class ViewModelLocator
    {
        private static IUnityContainer _container;

        public static void Configure(IUnityContainer container)
            => _container = container;

        public StartupViewModel Startup => _container.Resolve<StartupViewModel>();
        public MainViewModel Main => _container.Resolve<MainViewModel>();
        public SettingsViewModel Settings => _container.Resolve<SettingsViewModel>();
        public BookingsViewModel Bookings => _container.Resolve<BookingsViewModel>();
    }
}
