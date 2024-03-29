﻿using Cineworld.Api;
using CineworldAlerter.Core.Services;
using Unity;
using Unity.Lifetime;

namespace CineworldAlerter.Core
{
    public static class CoreUnityConfig
    {
        public static void Configure(IUnityContainer container)
        {
            //container.RegisterType<IApiClient, ApiClient>();
            container.RegisterType(typeof(ICachingService<>), typeof(CachingService<>));
            container.RegisterType(typeof(ICachingService<,>), typeof(CachingService<,>));
            container.RegisterType<ICinemaService, CinemaService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFilmService, FilmService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserPreferencesService, UserPreferencesService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IToastService, ToastService>();
            container.RegisterType<IBookingService, BookingService>(new ContainerControlledLifetimeManager());
        }
    }
}
