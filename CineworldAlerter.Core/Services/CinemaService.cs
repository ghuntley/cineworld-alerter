using System.Collections.Generic;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using Cineworld.Api;
using Cineworld.Api.Model;
using Newtonsoft.Json;

namespace CineworldAlerter.Core.Services
{
    public class CinemaService : ICinemaService
    {
        private const string CurrentCinemaKey = "CurrentCinema";
        private const string CinemaCacheFile = "CinemaCache.json";

        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IApiClient _apiClient;
        private readonly ICachingService<List<Cinema>> _cinemaCache;
        private readonly IStorageService _storageService;

        public Cinema CurrentCinema { get; private set; }

        public CinemaService(
            IApplicationSettingsService applicationSettingsService,
            IApiClient apiClient,
            ICachingService<List<Cinema>> cinemaCache,
            IStorageService storageService)
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
                _applicationSettingsService.Local.Remove(CurrentCinemaKey);
                return;
            }

            CurrentCinema = cinema;
            var json = JsonConvert.SerializeObject(cinema);
            _applicationSettingsService.Local.Set(CurrentCinemaKey, json);
        }

        public Task<List<Cinema>> GetCurrentCinemas() 
            => _cinemaCache.Get();

        private async Task<List<Cinema>> LoadCinemasFromFileOrApi()
        {
            if (!await _storageService.Local.FileExistsAsync(CinemaCacheFile))
            {
                var cinemas = await _apiClient.GetCinemas();
                var json = JsonConvert.SerializeObject(cinemas);
                await _storageService.Local.WriteAllTextAsync(CinemaCacheFile, json);

                return cinemas;
            }

            var fileContents = await _storageService.Local.ReadAllTextAsync(CinemaCacheFile);
            var cinemaList = JsonConvert.DeserializeObject<List<Cinema>>(fileContents);
            return cinemaList ?? new List<Cinema>();
        }

        private void LoadCinema()
        {
            if (!_applicationSettingsService.Local.Contains(CurrentCinemaKey))
                return;

            var json = _applicationSettingsService.Local.Get<string>(CurrentCinemaKey);
            if (string.IsNullOrEmpty(json))
            {
                _applicationSettingsService.Local.Remove(CurrentCinemaKey);
                return;
            }

            var cinema = JsonConvert.DeserializeObject<Cinema>(json);
            ChangeCinema(cinema);
        }
    }
}