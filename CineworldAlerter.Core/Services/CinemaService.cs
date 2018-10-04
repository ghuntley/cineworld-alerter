using Cimbalino.Toolkit.Services;
using Cineworld.Api.Model;
using Newtonsoft.Json;

namespace CineworldAlerter.Core.Services
{
    public class CinemaService : ICinemaService
    {
        private const string CurrentCinemaKey = "CurrentCinema";

        private readonly IApplicationSettingsService _applicationSettingsService;
        public Cinema CurrentCinema { get; private set; }

        public CinemaService(
            IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;

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