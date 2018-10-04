using Cineworld.Api.Model;

namespace CineworldAlerter.ViewModels.Entities
{
    public class FilmViewModel
    {
        private readonly FullFilm _film;

        public FilmViewModel(FullFilm film)
        {
            _film = film;
        }

        public string ImageUrl => $"https://www.cineworld.co.uk{_film.PosterSrc}";

        public string Name => _film.FeatureTitle;
    }
}
