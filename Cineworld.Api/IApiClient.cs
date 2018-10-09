using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cineworld.Api.Model;

namespace Cineworld.Api
{
    public interface IApiClient
    {
        Task<List<Cinema>> GetCinemas(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<CinemaFilm>> GetFilmsForCinema(string cinemaId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<FullFilm>> GetAllFilms(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<FullFilm>> SearchUnlimitedFilms(CancellationToken cancellationToken = default(CancellationToken));
    }
}