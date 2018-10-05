using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using CineworldAlerter.Core.Services;
using Unity;

namespace CineworldAlerter.Background
{
    public class TimedRefreshBackgroundTask
    {
        public async Task Run(IBackgroundTaskInstance taskInstance, IUnityContainer unityContainer)
        {
            try
            {
                var container = unityContainer ?? App.ConfigureContainer(false);

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
        }
    }
}
