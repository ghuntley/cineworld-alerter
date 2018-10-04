﻿using Cimbalino.Toolkit.Services;
using CineworldAlerter.Core.Services;
using CineworldAlerter.Services;
using Unity;
using Unity.Lifetime;

namespace CineworldAlerter
{
    public static class UwpUnityConfig
    {
        public static void Configure(IUnityContainer container)
        {
            container.RegisterInstance<INavigationService>(new NavigationService(), new ContainerControlledLifetimeManager());
            container.RegisterType<ICineworldNavigationService, CineworldNavigationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStorageService, StorageService>();
            container.RegisterType<IApplicationSettingsService, ApplicationSettingsService>();
        }
    }
}
