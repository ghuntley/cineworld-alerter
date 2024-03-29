﻿using Cimbalino.Toolkit.Services;
using Cineworld.Api;
using CineworldAlerter.Core.Services;
using CineworldAlerter.Core.Services.Local;
using CineworldAlerter.Windows.Core.SampleData;
using CineworldAlerter.Windows.Core.Services;
using CineworldAlerter.Windows.Core.Services.Local;
using Unity;

namespace CineworldAlerter.Windows.Core
{
    public static class UwpCoreUnityConfig
    {
        public static void Configure(IUnityContainer container)
        {
            //container.RegisterType<IApiClient, SampleDataApiClient>();
            container.RegisterType<IApiClient, ApiClient>();
            container.RegisterType<IStorageService, StorageService>();
            container.RegisterType<IApplicationSettingsService, ApplicationSettingsService>();
            container.RegisterType<IToastProxyService, ToastProxyService>();
            container.RegisterType<ILocalStorageService, WindowsLocalStorageService>();
            container.RegisterType<ILocalSettingsService, WindowsLocalSettingsService>();
        }
    }
}
