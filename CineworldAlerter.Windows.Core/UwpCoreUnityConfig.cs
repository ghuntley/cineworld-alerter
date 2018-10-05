using Cimbalino.Toolkit.Services;
using Cineworld.Api;
using CineworldAlerter.Core.Services;
using CineworldAlerter.Windows.Core.Services;
using Unity;

namespace CineworldAlerter.Windows.Core
{
    public static class UwpCoreUnityConfig
    {
        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<IApiClient, ApiClient>();
            container.RegisterType<IStorageService, StorageService>();
            container.RegisterType<IApplicationSettingsService, ApplicationSettingsService>();
            container.RegisterType<IToastService, ToastService>();
        }
    }
}
