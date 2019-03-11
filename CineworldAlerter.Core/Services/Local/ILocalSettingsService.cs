namespace CineworldAlerter.Core.Services.Local
{
    public interface ILocalSettingsService
    {
        void Remove(string key);
        void Set<T>(string key, T content);
        T Get<T>(string key);
        T Get<T>(string key, T defaultValue);
        bool Contains(string key);
    }
}