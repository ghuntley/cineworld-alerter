using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cineworld.Api.Extensions;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Extensions;

namespace CineworldAlerter.Core.Services
{
    public class ToastService : IToastService
    {
        private readonly IUserPreferencesService _userPreferencesService;
        private readonly IToastProxyService _toastProxyService;
        public const string LauncherCode = "L#";

        public ToastService(
            IUserPreferencesService userPreferencesService,
            IToastProxyService toastProxyService)
        {
            _userPreferencesService = userPreferencesService;
            _toastProxyService = toastProxyService;
        }

        public Task DisplayToasts(IEnumerable<FullFilm> films)
            => Task.Run(async () =>
            {
                foreach (var film in films)
                {
                    if (!CanFilmBeDisplayedInternal(true, film))
                        continue;

                    //await _toastProxyService.ShowToast(film, film.IsUnlimitedScreening());
                }
            });

        public Task AnnounceUnlimitedScreenings(IEnumerable<FullFilm> films)
            => Task.Run(async () =>
            {
                foreach (var film in films)
                {
                    //await _toastProxyService.AnnounceUnlimitedScreening(film);
                }
            });

        public bool CanFilmBeDisplayed(FullFilm film)
            => CanFilmBeDisplayedInternal(false, film);

        private bool CanFilmBeDisplayedInternal(bool isForToast, FullFilm film)
        {
            if (!isForToast && !_userPreferencesService.NotificationsFilterAppliesToList)
                return true;

            if (_userPreferencesService.AlertOnEverything
                || _userPreferencesService.DontShowAlertsFor.IsNullOrEmpty()
                || film.Attributes.IsNullOrEmpty())
                return true;

            return !film.Attributes.Any(x => _userPreferencesService.DontShowAlertsFor.Contains(x));
        }
    }
}