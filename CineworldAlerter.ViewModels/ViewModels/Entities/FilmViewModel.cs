using System.Linq;
using Cineworld.Api.Extensions;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Extensions;
using CineworldAlerter.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace CineworldAlerter.ViewModels.Entities
{
    public class FilmViewModel
    {
        private readonly FullFilm _film;

        public string ImageUrl => _film.PosterSrc.ToCineworldLink();

        public string Name => _film.FeatureTitle;

        public string ReleaseDate => _film.DateStarted?.ToString("D");

        public string Tooltip => $"{Name}\n\nReleased: {ReleaseDate}";

        public string RatingImageUrl => $"/xmedia/img/10108/rating/{GetFilmCode()}.png".ToCineworldLink();

        public string RunTime
        {
            get
            {
                var runtime = _film.FilmLength;
                if (runtime <= 0)
                    return null;

                return $"Runtime: {runtime} minutes";
            }
        }

        public string ReleaseDateLong
        {
            get
            {
                var date = _film.DateStarted;
                if (!date.HasValue)
                    return null;

                return $"Release date: {date.Value:dddd dd MMMM}";
            }
        }

        public FilmViewModel(FullFilm film) 
            => _film = film;

        public void LaunchBooking()
            => Messenger.Default.Send(new FilmChangedMessage(_film));

        public void ViewFilmOnWebsite()
            => Messenger.Default.Send(new NotificationMessage(_film.Url.ToCineworldLink()));

        private string GetFilmCode()
            => _film.Attributes
                .FirstOrDefault(x => x.IsRating())
                .GetCode()
                .ToUpperInvariant();
    }
}
