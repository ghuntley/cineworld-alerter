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
            => Task.Run(() =>
            {
                foreach (var film in films)
                {
                    if (!ShowToastForFilm(film))
                        continue;

                    _toastProxyService.ShowToast(film, film.IsUnlimitedScreening());
                }
            });

        private bool ShowToastForFilm(FullFilm film)
        {
            if (_userPreferencesService.AlertOnEverything
                || _userPreferencesService.DontShowAlertsFor.IsNullOrEmpty()
                || film.Attributes.IsNullOrEmpty())
                return true;

            return !film.Attributes.Any(x => _userPreferencesService.DontShowAlertsFor.Contains(x));
        }
    }
}