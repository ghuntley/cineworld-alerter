using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cimbalino.Toolkit.Services;
using Cineworld.Api.Model;

namespace CineworldAlerter.Core.Services
{
    public interface IUserPreferencesService
    {
        bool AlertOnEverything { get; set; }

        IEnumerable<FilmCategory> DontShowAlertsFor { get; set; }

        void Save();
    }

    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly IApplicationSettingsService _applicationSettingsService;

        private IApplicationSettingsServiceHandler LocalSettings => _applicationSettingsService.Local;

        public UserPreferencesService(
            IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;

            Load();
        }

        public bool AlertOnEverything { get; set; }

        public IEnumerable<FilmCategory> DontShowAlertsFor { get; set; }

        public void Save()
        {
            LocalSettings.Set(GetContainerKey(nameof(AlertOnEverything)), AlertOnEverything);
            LocalSettings.Set(GetContainerKey(nameof(DontShowAlertsFor)), DontShowAlertsFor);
        }

        private void Load()
        {
            AlertOnEverything = LocalSettings.Get(GetContainerKey(nameof(AlertOnEverything)), true);
            DontShowAlertsFor = LocalSettings.Get<IEnumerable<FilmCategory>>(GetContainerKey(nameof(DontShowAlertsFor)));
        }

        private string GetContainerKey(string key)
            => $"{nameof(UserPreferencesService)}\\{key}";
    }
}
