using System.Collections.Generic;
using System.Threading.Tasks;
using Cineworld.Api.Model;

namespace CineworldAlerter.Core.Services
{
    public interface IFilmService
    {
        Task<List<FullFilm>> GetLocalFilms();
        Task<List<FullFilm>> GetAllFilms();
        Task RefreshFilms(string cinemaId);
    }
}