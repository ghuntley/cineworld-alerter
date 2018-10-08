using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Extensions;

namespace CineworldAlerter.ViewModels.Entities
{
    public class FilmViewModel
    {
        private FullFilm _film;
        private readonly ILauncherService _launcherService;

        public FilmViewModel(
            ILauncherService launcherService)
        {
            _launcherService = launcherService;
        }

        public string ImageUrl => _film.PosterSrc.ToCineworldLink();

        public string Name => _film.FeatureTitle;

        public void LaunchBooking()
            => _launcherService.LaunchUriAsync(_film.Url.ToCineworldLink());

        public FilmViewModel WithFilm(FullFilm film)
        {
            _film = film;
            return this;
        }
    }
}
