using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Cimbalino.Toolkit.Handlers;
using Cimbalino.Toolkit.Services;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Extensions;
using CineworldAlerter.Core.Services;
using CineworldAlerter.ViewModels.Entities;
using GalaSoft.MvvmLight;

namespace CineworldAlerter.ViewModels
{
    public class MainViewModel : ViewModelBase, IHandleNavigatedTo
    {
        private readonly IFilmService _filmService;
        private readonly ICinemaService _cinemaService;
        private readonly IBackgroundLauncherService _backgroundLauncherService;
        private readonly ICineworldNavigationService _navigationService;
        private readonly IUserPreferencesService _userPreferencesService;

        private bool _isLoading;
        private Cinema _selectedCinema;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (Set(ref _isLoading, value))
                    RaisePropertyChanged(nameof(CanRefresh));
            }
        }

        public Cinema SelectedCinema
        {
            get => _selectedCinema;
            set
            {
                if (Set(ref _selectedCinema, value))
                    UpdateCinema(value).DontAwait();
            }
        }

        public bool CanRefresh => !IsLoading;

        public string CinemaName => _cinemaService.CurrentCinema.DisplayName;

        public ObservableCollection<FilmViewModel> Films { get; set; }
            = new ObservableCollection<FilmViewModel>();

        public ObservableCollection<Cinema> Cinemas { get; }
            = new ObservableCollection<Cinema>();

        public MainViewModel(
            IFilmService filmService,
            ICinemaService cinemaService,
            IBackgroundLauncherService backgroundLauncherService,
            ICineworldNavigationService navigationService,
            IUserPreferencesService userPreferencesService)
        {
            _filmService = filmService;
            _cinemaService = cinemaService;
            _backgroundLauncherService = backgroundLauncherService;
            _navigationService = navigationService;
            _userPreferencesService = userPreferencesService;

            _userPreferencesService.UserPreferencesChanged -= UserPreferencesServiceOnUserPreferencesChanged;
            _userPreferencesService.UserPreferencesChanged += UserPreferencesServiceOnUserPreferencesChanged;
        }

        private void UserPreferencesServiceOnUserPreferencesChanged(object sender, EventArgs e)
            => LoadData(true).DontAwait();

        public async Task OnNavigatedToAsync(NavigationServiceNavigationEventArgs eventArgs)
        {
            if (eventArgs.NavigationMode == NavigationServiceNavigationMode.Back)
                return;

            _backgroundLauncherService.Startup().DontAwait();

            await LoadData();

            LoadCinemas().DontAwait();
        }

        public void NavigateToSettings()
            => _navigationService.NavigateToSettingsPage();

        public async void Refresh()
            => await LoadData(true);

        private async Task LoadData(bool isRefresh = false)
        {
            IsLoading = true;

            if(isRefresh)
                Films.Clear();

            await _filmService.RefreshFilms(_cinemaService.CurrentCinema.Id);

            var films = await _filmService.GetLocalFilms();
            var filmViewModels = films.Select(x => new FilmViewModel(x));

            Films.AddRange(filmViewModels);

            IsLoading = false;
        }

        private async Task LoadCinemas()
        {
            var cinemas = await _cinemaService.GetCurrentCinemas();
            Cinemas.Clear();
            Cinemas.AddRange(cinemas);

            _selectedCinema = Cinemas.FirstOrDefault(x => x.Id == _cinemaService.CurrentCinema.Id);
            RaisePropertyChanged(nameof(SelectedCinema));
        }

        private async Task UpdateCinema(Cinema cinema)
        {
            _cinemaService.ChangeCinema(cinema);
            await _filmService.DeleteLocalFilms();
            await LoadData(true);

            RaisePropertyChanged(nameof(CinemaName));
        }
    }
}
