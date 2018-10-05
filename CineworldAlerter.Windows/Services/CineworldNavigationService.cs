using Cimbalino.Toolkit.Services;
using CineworldAlerter.Core.Services;
using CineworldAlerter.Views;

namespace CineworldAlerter.Services
{
    public class CineworldNavigationService : ICineworldNavigationService
    {
        private readonly INavigationService _navigationService;

        public CineworldNavigationService(
            INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool NavigateToMainPage(object parameter = null) 
            => _navigationService.Navigate<MainView>(parameter);

        public void ClearBackStack() 
            => _navigationService.ClearBackstack();
    }
}
