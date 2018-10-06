using Cineworld.Api.Model;

namespace Cineworld.Api.Extensions
{
    public static class FullFilmExtensions
    {
        public static bool IsUnlimitedScreening(this FullFilm film)
            => film.Attributes?.Contains(FilmCategory.UnlimitedScreening) ?? false;
    }
}
