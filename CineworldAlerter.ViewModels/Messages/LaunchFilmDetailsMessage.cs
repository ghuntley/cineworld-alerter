using GalaSoft.MvvmLight.Messaging;

namespace CineworldAlerter.Messages
{
    public class LaunchFilmDetailsMessage : MessageBase
    {
        public string FilmLink { get; }

        public LaunchFilmDetailsMessage(string filmLink)
            => FilmLink = filmLink;
    }
}