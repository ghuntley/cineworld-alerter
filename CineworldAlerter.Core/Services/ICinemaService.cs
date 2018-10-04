using Cineworld.Api.Model;

namespace CineworldAlerter.Core.Services
{
    public interface ICinemaService
    {
        Cinema CurrentCinema { get; }

        void ChangeCinema(Cinema cinema);
    }
}
