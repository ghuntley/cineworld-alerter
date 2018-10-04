using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Cimbalino.Toolkit.Handlers;
using Cimbalino.Toolkit.Services;
using CineworldAlerter.Core.Services;
using CineworldAlerter.ViewModels.Entities;
using GalaSoft.MvvmLight;

namespace CineworldAlerter.ViewModels
{
    public class MainViewModel : ViewModelBase, IHandleNavigatedTo
    {
        private readonly IFilmService _filmService;
        private readonly ICinemaService _cinemaService;

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (Set(ref _isLoading, value))
                    RaisePropertyChanged(nameof(IsLoading));
            }
        }

        public bool CanRefresh => !IsLoading;

        public string CinemaName => _cinemaService.CurrentCinema.DisplayName;

        public ObservableCollection<FilmViewModel> Films { get; set; }
            = new ObservableCollection<FilmViewModel>();

        public MainViewModel(
            IFilmService filmService,
            ICinemaService cinemaService)
        {
            _filmService = filmService;
            _cinemaService = cinemaService;
        }

        public async Task OnNavigatedToAsync(NavigationServiceNavigationEventArgs eventArgs)
        {
            if (eventArgs.NavigationMode == NavigationServiceNavigationMode.Back)
                return;

            await LoadData();
        }

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
    }
}
