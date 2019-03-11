using Cimbalino.Toolkit.Services;
using CineworldAlerter.Core.Services.Local;

namespace CineworldAlerter.Windows.Core.Services.Local
{
    public class WindowsLocalSettingsService :ILocalSettingsService
    {
        private readonly IApplicationSettingsService _settingsService;

        public WindowsLocalSettingsService(
            IApplicationSettingsService settingsService) 
            => _settingsService = settingsService;

        public void Remove(string key) 
            => _settingsService.Local.Remove(key);

        public void Set<T>(string key, T content) 
            => _settingsService.Local.Set(key, content);

        public T Get<T>(string key) 
            => _settingsService.Local.Get<T>(key);

        public T Get<T>(string key, T defaultValue) 
            => _settingsService.Local.Get(key, defaultValue);

        public bool Contains(string key)
            => _settingsService.Local.Contains(key);
    }
}