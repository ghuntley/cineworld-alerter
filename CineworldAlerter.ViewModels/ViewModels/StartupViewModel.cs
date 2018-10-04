using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Cimbalino.Toolkit.Handlers;
using Cimbalino.Toolkit.Services;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services;
using GalaSoft.MvvmLight;

namespace CineworldAlerter.ViewModels
{
    public class StartupViewModel : ViewModelBase, IHandleNavigatedTo
    {
        private readonly ICineworldNavigationService _navigationService;
        private readonly ICinemaService _cinemaService;

        private Cinema _selectedCinema;

        public ObservableCollection<Cinema> Cinemas { get; set; }
            = new ObservableCollection<Cinema>();

        public Cinema SelectedCinema
        {
            get => _selectedCinema;
            set
            {
                if(Set(ref _selectedCinema, value))
                    RaisePropertyChanged(nameof(CanMoveOn));
            }
        }

        public bool CanMoveOn => SelectedCinema != null;

        public StartupViewModel(
            ICineworldNavigationService navigationService,
            ICinemaService cinemaService)
        {
            _navigationService = navigationService;
            _cinemaService = cinemaService;
        }

        public async Task OnNavigatedToAsync(NavigationServiceNavigationEventArgs eventArgs)
        {
            if (_cinemaService.CurrentCinema != null)
            {
                await Task.Delay(10);

                _navigationService.NavigateToMainPage();
                _navigationService.ClearBackStack();
                return;
            }

            Cinemas.Clear();

            var cinemas = await _cinemaService.GetCurrentCinemas();
            Cinemas.AddRange(cinemas);
        }

        public void SetCinema()
        {
            _cinemaService.ChangeCinema(SelectedCinema);
            _navigationService.NavigateToMainPage();
            _navigationService.ClearBackStack();
        }
    }
}
