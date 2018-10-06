using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Cimbalino.Toolkit.Handlers;
using Cimbalino.Toolkit.Services;
using Cineworld.Api.Extensions;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services;
using CineworldAlerter.ViewModels.Entities;
using GalaSoft.MvvmLight;

namespace CineworldAlerter.ViewModels
{
    public class SettingsViewModel : ViewModelBase, IHandleNavigatedTo, IHandleNavigatingFrom
    {
        private readonly IUserPreferencesService _userPreferencesService;

        private bool _showMeEverything;

        public ObservableCollection<FilmCategoryViewModel> FilmRatings { get; }
            = new ObservableCollection<FilmCategoryViewModel>();

        public ObservableCollection<FilmCategoryViewModel> PersonBasedCategories { get; }
            = new ObservableCollection<FilmCategoryViewModel>();

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
            if(eventArgs.NavigationMode == NavigationServiceNavigationMode.Back)
                return Task.CompletedTask;

            var categories = EnumHelper.GetValues<FilmCategory>();

            ConfigureRatings(categories);

            _showMeEverything = _userPreferencesService.AlertOnEverything;
            return Task.CompletedTask;
        }

        private void ConfigureRatings(IEnumerable<FilmCategory> categories)
        {
            var ignoredRatings = _userPreferencesService.DontShowAlertsFor
                .Where(x => x.IsRating())
                .ToList();

            var ratings = categories
                .Where(x => x.IsRating())
                .Select(x => new FilmCategoryViewModel(x)
                {
                    DontAlertMe = ignoredRatings.Contains(x)
                });

            FilmRatings.Clear();
            FilmRatings.AddRange(ratings);
        }

        public Task OnNavigatingFromAsync(NavigationServiceNavigatingCancelEventArgs eventArgs)
        {
            var ratingsChanges = FilmRatings
                .Where(x => x.DontAlertMe)
                .Select(x => x.FilmCategory);

            var personChanges = PersonBasedCategories
                .Where(x => x.DontAlertMe)
                .Select(x => x.FilmCategory);

            var allCategories = ratingsChanges
                .Union(personChanges)
                .Distinct();

            _userPreferencesService.DontShowAlertsFor = allCategories;

            _userPreferencesService.Save();
            return Task.CompletedTask;
        }
    }
}
