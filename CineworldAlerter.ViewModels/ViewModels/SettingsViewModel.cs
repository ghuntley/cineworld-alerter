using System.Threading.Tasks;
using Cimbalino.Toolkit.Handlers;
using Cimbalino.Toolkit.Services;
using CineworldAlerter.Core.Services;
using GalaSoft.MvvmLight;

namespace CineworldAlerter.ViewModels
{
    public class SettingsViewModel : ViewModelBase, IHandleNavigatedTo, IHandleNavigatingFrom
    {
        private readonly IUserPreferencesService _userPreferencesService;

        private bool _showMeEverything;

        public bool ShowMeEverything
        {
            get => _showMeEverything;
            set
            {
                if (Set(ref _showMeEverything, value))
                    _userPreferencesService.AlertOnEverything = value;
            }
        }

        public SettingsViewModel(
            IUserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService;
        }

        public Task OnNavigatedToAsync(NavigationServiceNavigationEventArgs eventArgs)
        {
            _showMeEverything = _userPreferencesService.AlertOnEverything;
            return Task.CompletedTask;
        }

        public Task OnNavigatingFromAsync(NavigationServiceNavigatingCancelEventArgs eventArgs)
        {
            _userPreferencesService.Save();
            return Task.CompletedTask;
        }
    }
}
