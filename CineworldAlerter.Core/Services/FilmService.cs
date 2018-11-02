using Cimbalino.Toolkit.Services;
using Cineworld.Api;
using Cineworld.Api.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineworldAlerter.Core.Extensions;

namespace CineworldAlerter.Core.Services
{
    public class FilmService : IFilmService
    {
        public const string FilmCacheFile = "CinemaFilmCache.json";
        public const string AllFilmCacheFile = "AllFilmsCache.json";
        public const string UnlimitedScreeningsCacheFile = "UnlimitedScreenings.json";

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

            await CheckForUnlimitedScreenings();

            var isFirstRun = !localFilms.Any();

            var existingLocalFilms = localFilms
                .Where(x => externalFilms.Any(y => y.FilmId == x.Code))
                .ToList();

            var localFilmsChanged = CorrectWeightingOfExistingFilms(existingLocalFilms, allFilms);

            var existingExternalFilms = externalFilms
                .Where(x => localFilms.Any(y => y.Code == x.FilmId))
                .ToList();

            var filmsToRemove = localFilms
                .Where(x => existingLocalFilms.All(y => y.Code != x.Code))
                .ToList();

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
                if (!isFirstRun)
                {
                    await _toastService.DisplayToasts(addedLocalFilms);
                }

                localFilms.AddRange(addedLocalFilms);
                localFilmsChanged = true;
            }

            if (localFilmsChanged)
            {
                localFilms = localFilms.OrderBy(x => x.Weight).ToList();
                await WriteFilmsToFile(FilmCacheFile, localFilms);
                _filmCache.ClearCache(FilmCacheFile);
            }

            await WriteFilmsToFile(AllFilmCacheFile, allFilms);
        }

        private static bool CorrectWeightingOfExistingFilms(
            IEnumerable<FullFilm> existingLocalFilms,
            IReadOnlyCollection<FullFilm> allFilms)
        {
            var listChanged = false;

            foreach (var film in existingLocalFilms)
            {
                var externalFilm = allFilms.FirstOrDefault(x => x.Code == film.Code);
                if (externalFilm == null || film.Weight == externalFilm.Weight) continue;

                listChanged = true;
                film.Weight = externalFilm.Weight;
            }

            return listChanged;
        }

        public async Task DeleteLocalFilms()
        {
            if (!await _storageService.Local.FileExistsAsync(FilmCacheFile))
                return;

            await _storageService.Local.DeleteFileAsync(FilmCacheFile);
            _filmCache.ClearCache(FilmCacheFile);
        }

        public async Task CheckForUnlimitedScreenings()
        {
            var externalUnlimitedFilms = await _apiClient.SearchUnlimitedFilms();
            var localUnlimitedFilms = await _filmCache.Get(UnlimitedScreeningsCacheFile);

            var isFirstRun = !localUnlimitedFilms.Any();

            var existingLocalFilms = localUnlimitedFilms
                .Where(x => externalUnlimitedFilms.Any(y => y.FeatureTitle == x.FeatureTitle))
                .ToList();

            var existingExternalFilms = externalUnlimitedFilms
                .Where(x => localUnlimitedFilms.Any(y => y.FeatureTitle == x.FeatureTitle))
                .ToList();

            var filmsToRemove = localUnlimitedFilms
                .Where(x => existingLocalFilms.All(y => y.FeatureTitle != x.FeatureTitle))
                .ToList();

            var localFileChanged = false;

            if (filmsToRemove.Any())
            {
                RemoveFilms(localUnlimitedFilms, filmsToRemove);
                localFileChanged = true;
            }

            var addedExternalFilms = externalUnlimitedFilms
                .Except(existingExternalFilms)
                .ToList();

            if (addedExternalFilms.Any())
            {
                if (!isFirstRun)
                {
                    await _toastService.AnnounceUnlimitedScreenings(addedExternalFilms);
                }

                localUnlimitedFilms.AddRange(addedExternalFilms);
                localFileChanged = true;
            }

            if (localFileChanged)
            {
                await WriteFilmsToFile(UnlimitedScreeningsCacheFile, localUnlimitedFilms);
                _filmCache.ClearCache(UnlimitedScreeningsCacheFile);
            }
        }

        private static void RemoveFilms(
            ICollection<FullFilm> localFilms,
            IEnumerable<FullFilm> filmsToRemove)
        {
            foreach (var film in filmsToRemove)
                localFilms.Remove(film);
        }

        private async Task<List<FullFilm>> LoadFilmsFromFile(string fileName)
        {
            if (!await _storageService.Local.FileExistsAsync(fileName))
                return new List<FullFilm>();

            var json = await _storageService.Local.ReadAllTextAsync(fileName);
            var filmList = JsonConvert.DeserializeObject<List<FullFilm>>(json);
            return filmList?.OrderBy(x => x.Weight).ToList() ?? new List<FullFilm>();
        }

        private async Task WriteFilmsToFile(string file, IEnumerable<FullFilm> films)
        {
            var json = JsonConvert.SerializeObject(films);
            await _storageService.Local.WriteAllTextAsync(file, json);
        }
    }
}