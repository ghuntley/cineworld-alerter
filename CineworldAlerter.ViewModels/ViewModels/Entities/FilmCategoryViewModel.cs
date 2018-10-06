using Cineworld.Api.Extensions;
using Cineworld.Api.Model;
using GalaSoft.MvvmLight;

namespace CineworldAlerter.ViewModels.Entities
{
    public class FilmCategoryViewModel : ViewModelBase
    {
        private bool _dontAlertMe;

        public FilmCategory FilmCategory { get; }

        public bool DontAlertMe
        {
            get => _dontAlertMe;
            set => Set(ref _dontAlertMe, value);
        }

        public FilmCategoryViewModel(FilmCategory filmCategory)
        {
            FilmCategory = filmCategory;
        }

        public string Name => FilmCategory.GetDisplayName();
    }
}