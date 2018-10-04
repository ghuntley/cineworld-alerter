using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using Cineworld.Api;
using Cineworld.Api.Model;
using Newtonsoft.Json;

namespace CineworldAlerter.Core.Services
{
    public interface IFilmService
    {
        Task<List<FullFilm>> GetLocalFilms();
        Task<List<FullFilm>> GetAllFilms();
        Task RefreshFilms(string cinemaId);
    }

    public class FilmService : IFilmService
    {
        public const string FilmCacheFile = "CinemaFilmCache.json";
        public const string AllFilmCacheFile = "AllFilmsCache.json";

        private readonly IApiClient _apiClient;
        private readonly ICachingService<string, List<FullFilm>> _filmCache;
        private readonly IStorageService _storageService;
        private readonly IToastService _toastService;

        public FilmService(
            IApiClient apiClient,
            ICachingService<string, List<FullFilm>> filmCache,
            IStorageService storageService,
            IToastService toastService)
        {
            _apiClient = apiClient;
            _filmCache = filmCache;
            _storageService = storageService;
            _toastService = toastService;

            _filmCache.Initialise(LoadFilmsFromFile);
        }

        public Task<List<FullFilm>> GetLocalFilms() 
            => _filmCache.Get(FilmCacheFile);

        public Task<List<FullFilm>> GetAllFilms()
            => _filmCache.Get(AllFilmCacheFile);

        public async Task RefreshFilms(string cinemaId)
        {
            var externalFilms = await _apiClient.GetFilmsForCinema(cinemaId);
            var localFilms = await GetLocalFilms();
            var allFilms = await _apiClient.GetAllFilms();

            var existingLocalFilms = localFilms
                .Where(x => externalFilms.Any(y => y.FilmId == x.Code))
                .ToList();

            var existingExternalFilms = externalFilms
                .Where(x => localFilms.Any(y => y.Code == x.FilmId))
                .ToList();

            var filmsToRemove = localFilms
                .Where(x => existingLocalFilms.All(y => y.Code != x.Code))
                .ToList();

            var localFilmsChanged = false;

            if (filmsToRemove.Any())
            {
                RemoveFilms(localFilms, filmsToRemove);
                localFilmsChanged = true;
            }
            
            var addedExternalFilms = externalFilms
                .Except(existingExternalFilms)
                .ToList();

            var addedLocalFilms = allFilms
                .Where(x => addedExternalFilms.Any(y => y.FilmId == x.Code))
                .ToList();

            if (addedExternalFilms.Any())
            {
                await _toastService.DisplayToasts(addedLocalFilms);
                localFilms.AddRange(addedLocalFilms);
                localFilmsChanged = true;
            }

            if (localFilmsChanged)
            {
                await WriteFilmsToFile(FilmCacheFile, localFilms);
                _filmCache.ClearCache();
            }

            await WriteFilmsToFile(AllFilmCacheFile, allFilms);
        }

        private void RemoveFilms(
            ICollection<FullFilm> localFilms, 
            IEnumerable<FullFilm> filmsToRemove)
        {
            foreach (var film in filmsToRemove)
                localFilms.Remove(film);
        }

        private async Task<List<FullFilm>> LoadFilmsFromFile(string fileName)
        {
            if(!await _storageService.Local.FileExistsAsync(fileName))
                return new List<FullFilm>();

            var json = await _storageService.Local.ReadAllTextAsync(fileName);
            var filmList = JsonConvert.DeserializeObject<List<FullFilm>>(json);
            return filmList ?? new List<FullFilm>();
        }

        private async Task WriteFilmsToFile(string file, IEnumerable<FullFilm> films)
        {
            var json = JsonConvert.SerializeObject(films);
            await _storageService.Local.WriteAllTextAsync(file, json);
        }
    }
}