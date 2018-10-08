using System;
using System.Collections.Generic;
using Cimbalino.Toolkit.Services;
using Cineworld.Api.Model;
using Newtonsoft.Json;

namespace CineworldAlerter.Core.Services
{
    public interface IUserPreferencesService
    {
        event EventHandler UserPreferencesChanged;

        bool AlertOnEverything { get; set; }

        IEnumerable<FilmCategory> DontShowAlertsFor { get; set; }

        bool NotificationsFilterAppliesToList { get; set; }

        void Save();
    }

    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly IApplicationSettingsService _applicationSettingsService;

        private IApplicationSettingsServiceHandler LocalSettings => _applicationSettingsService.Local;

        public event EventHandler UserPreferencesChanged;

        public UserPreferencesService(
            IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;

            Load();
        }

        public bool AlertOnEverything { get; set; }

        public IEnumerable<FilmCategory> DontShowAlertsFor { get; set; }
        public bool NotificationsFilterAppliesToList { get; set; }

        public void Save()
        {
            LocalSettings.Set(GetContainerKey(nameof(AlertOnEverything)), AlertOnEverything);
            LocalSettings.Set(GetContainerKey(nameof(DontShowAlertsFor)), JsonConvert.SerializeObject(DontShowAlertsFor));
            LocalSettings.Set(GetContainerKey(nameof(NotificationsFilterAppliesToList)), NotificationsFilterAppliesToList);

            UserPreferencesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Load()
        {
            AlertOnEverything = LocalSettings.Get(GetContainerKey(nameof(AlertOnEverything)), true);
            NotificationsFilterAppliesToList = LocalSettings.Get<bool>(GetContainerKey(nameof(NotificationsFilterAppliesToList)));

            var json = LocalSettings.Get<string>(GetContainerKey(nameof(DontShowAlertsFor)));
            DontShowAlertsFor = !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<List<FilmCategory>>(json)
                : new List<FilmCategory>();
        }

        private string GetContainerKey(string key)
            => $"{nameof(UserPreferencesService)}\\{key}";
    }
}
