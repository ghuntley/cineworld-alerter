using System;
using Windows.ApplicationModel.Background;
using CineworldAlerter.Core;
using CineworldAlerter.Core.Services;
using CineworldAlerter.Windows.Core;
using Unity;

namespace CineworldAlerter.Windows.BackgroundTasks
{
    public sealed class TimedRefreshBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                var container = ConfigureContainer();

                var filmService = container.Resolve<IFilmService>();
                var cinemaService = container.Resolve<ICinemaService>();

                if (cinemaService.CurrentCinema == null)
                    return;

                await filmService.RefreshFilms(cinemaService.CurrentCinema.Id);
            }
            catch (Exception ex)
            {
                // ignore for now
            }
            finally
            {
                deferral.Complete();
            }
        }
        
        private static IUnityContainer ConfigureContainer()
        {
            var container = new UnityContainer();
            CoreUnityConfig.Configure(container);
            UwpCoreUnityConfig.Configure(container);

            return container;
        }
    }
}
