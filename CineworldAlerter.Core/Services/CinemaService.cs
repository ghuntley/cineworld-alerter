using System.Collections.Generic;
using System.Threading.Tasks;
using Cineworld.Api;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services.Local;
using Newtonsoft.Json;

namespace CineworldAlerter.Core.Services
{
    public class CinemaService : ICinemaService
    {
        private const string CurrentCinemaKey = "CurrentCinema";
        private const string CinemaCacheFile = "CinemaCache.json";

        private readonly ILocalSettingsService _applicationSettingsService;
        private readonly IApiClient _apiClient;
        private readonly ICachingService<List<Cinema>> _cinemaCache;
        private readonly ILocalStorageService _storageService;

        public Cinema CurrentCinema { get; private set; }

        public CinemaService(
            ILocalSettingsService applicationSettingsService,
            IApiClient apiClient,
            ICachingService<List<Cinema>> cinemaCache,
            ILocalStorageService storageService)
        {
            _applicationSettingsService = applicationSettingsService;
            _apiClient = apiClient;
            _cinemaCache = cinemaCache;
            _storageService = storageService;

            _cinemaCache.Initialise(LoadCinemasFromFileOrApi);

            LoadCinema();
        }

        public void ChangeCinema(Cinema cinema)
        {
            if (cinema == null)
            {
                CurrentCinema = null;
                _applicationSettingsService.Remove(CurrentCinemaKey);
                return;
            }

            CurrentCinema = cinema;
            var json = JsonConvert.SerializeObject(cinema);
            _applicationSettingsService.Set(CurrentCinemaKey, json);
        }

        public Task<List<Cinema>> GetCurrentCinemas() 
            => _cinemaCache.Get();

        private async Task<List<Cinema>> LoadCinemasFromFileOrApi()
        {
            if (!await _storageService.FileExistsAsync(CinemaCacheFile))
            {
                var cinemas = await _apiClient.GetCinemas();
                var json = JsonConvert.SerializeObject(cinemas);
                await _storageService.WriteAllTextAsync(CinemaCacheFile, json);

                return cinemas;
            }

            var fileContents = await _storageService.ReadAllTextAsync(CinemaCacheFile);
            var cinemaList = JsonConvert.DeserializeObject<List<Cinema>>(fileContents);
            return cinemaList ?? new List<Cinema>();
        }

        private void LoadCinema()
        {
            if (!_applicationSettingsService.Contains(CurrentCinemaKey))
                return;

            var json = _applicationSettingsService.Get<string>(CurrentCinemaKey);
            if (string.IsNullOrEmpty(json))
            {
                _applicationSettingsService.Remove(CurrentCinemaKey);
                return;
            }

            var cinema = JsonConvert.DeserializeObject<Cinema>(json);
            ChangeCinema(cinema);
        }
    }
}