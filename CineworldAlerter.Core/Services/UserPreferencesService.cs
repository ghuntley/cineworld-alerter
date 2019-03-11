using System;
using System.Collections.Generic;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services.Local;
using Newtonsoft.Json;

namespace CineworldAlerter.Core.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        private ILocalSettingsService LocalSettings { get; }

        public event EventHandler UserPreferencesChanged;

        public UserPreferencesService(
            ILocalSettingsService applicationSettingsService)
        {
            LocalSettings = applicationSettingsService;

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