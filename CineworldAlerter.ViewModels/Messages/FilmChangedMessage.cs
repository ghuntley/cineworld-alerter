using Cineworld.Api.Model;
using CineworldAlerter.ViewModels.Entities;
using GalaSoft.MvvmLight.Messaging;

namespace CineworldAlerter.Messages
{
    public class FilmChangedMessage : MessageBase
    {
        public FullFilm Film { get; }

        public FilmChangedMessage(FullFilm film) 
            => Film = film;
    }
}
