using System.Collections.Generic;
using System.Threading.Tasks;
using Cineworld.Api.Model;

namespace CineworldAlerter.Core.Services
{
    public interface ICinemaService
    {
        Cinema CurrentCinema { get; }

        void ChangeCinema(Cinema cinema);

        Task<List<Cinema>> GetCurrentCinemas();
    }
}
