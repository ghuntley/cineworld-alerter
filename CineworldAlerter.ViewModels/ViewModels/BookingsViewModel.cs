using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Extensions;
using CineworldAlerter.Core.Services;
using CineworldAlerter.ViewModels.Entities;
using GalaSoft.MvvmLight;

namespace CineworldAlerter.ViewModels
{
    public class BookingsViewModel : ViewModelBase
    {
        private readonly IBookingService _bookingService;
        private readonly ICinemaService _cinemaService;
        private readonly Func<BookingViewModel> _bookingViewModelFactory;

        private FilmViewModel _selectedFilm;
        private BookingDateViewModel _selectedDateOption;

        private string CinemaCode => _cinemaService.CurrentCinema.GroupId;

        public FilmViewModel SelectedFilm
        {
            get => _selectedFilm;
            set => Set(ref _selectedFilm, value);
        }

        public BookingDateViewModel SelectedDateOption
        {
            get => _selectedDateOption;
            set
            {
                if (Set(ref _selectedDateOption, value))
                    SelectedDateChanged().DontAwait();
            }
        }

        public ObservableCollection<BookingDateViewModel> Dates { get; }
            = new ObservableCollection<BookingDateViewModel>();

        public BookingsViewModel(
            IBookingService bookingService,
            ICinemaService cinemaService,
            Func<BookingViewModel> bookingViewModelFactory)
        {
            _bookingService = bookingService;
            _cinemaService = cinemaService;
            _bookingViewModelFactory = bookingViewModelFactory;
        }

        public async Task Initialise(FullFilm film)
        {
            SelectedFilm = new FilmViewModel(film);
            var dates = await _bookingService.GetDates(CinemaCode, film.Code);

            var dateOptions = dates.Select(x => new BookingDateViewModel(
                x, 
                film,
                _cinemaService.CurrentCinema,
                _bookingService,
                _bookingViewModelFactory));
            Dates.AddRange(dateOptions);

            if (Dates.Any())
                SelectedDateOption = Dates.First();
        }

        public void TearDown()
        {
            _selectedFilm = null;
            Dates.Clear();
        }

        private async Task SelectedDateChanged()
        {
            if (_selectedFilm == null || SelectedDateOption == null)
                return;

            await SelectedDateOption.Initialise();
        }
    }

    public class BookingDateViewModel : ViewModelBase
    {
        private readonly IBookingService _bookingService;
        private readonly Func<BookingViewModel> _bookingViewModelFactory;

        public DateTimeOffset Date { get; }
        public FullFilm Film { get; }

        public string Day => Date.ToString("ddd");
        public string DateString => Date.ToString("dd/MM");

        private string CinemaCode { get; }
    
        public ObservableCollection<BookingViewModel> Bookings { get; }
            = new ObservableCollection<BookingViewModel>();

        public BookingDateViewModel(
            DateTimeOffset date,
            FullFilm film,
            Cinema cinema,
            IBookingService bookingService,
            Func<BookingViewModel> bookingViewModelFactory)
        {
            _bookingService = bookingService;
            _bookingViewModelFactory = bookingViewModelFactory;
            Date = date;
            Film = film;
            CinemaCode = cinema.GroupId;
        }

        public async Task Initialise()
        {
            Bookings.Clear();

            var bookings = await _bookingService.GetBookings(CinemaCode, Film.Code, Date);
            var bookingsViewModels = bookings.Select(x => _bookingViewModelFactory().WithBooking(x));

            Bookings.AddRange(bookingsViewModels);
        }
    }
}
