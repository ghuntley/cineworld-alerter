﻿using Cimbalino.Toolkit.Services;
using Cineworld.Api;
using CineworldAlerter.Core.Services;
using CineworldAlerter.SampleData;
using CineworldAlerter.Services;
using Unity;
using Unity.Lifetime;

namespace CineworldAlerter
{
    public static class UwpUnityConfig
    {
        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<IApiClient, SampleDataApiClient>();
            container.RegisterInstance<INavigationService>(new NavigationService(), new ContainerControlledLifetimeManager());
            container.RegisterType<ICineworldNavigationService, CineworldNavigationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStorageService, StorageService>();
            container.RegisterType<IApplicationSettingsService, ApplicationSettingsService>();
            container.RegisterType<IToastService, ToastService>();
            container.RegisterType<IBackgroundLauncherService, BackgroundLauncherService>();
        }
    }
}
