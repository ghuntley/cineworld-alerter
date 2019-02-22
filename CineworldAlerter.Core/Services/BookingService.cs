using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cineworld.Api;
using Cineworld.Api.Model;

namespace CineworldAlerter.Core.Services
{
    public class BookingService: IBookingService
    {
        private readonly ICachingService<(string cinema, string film), List<DateTimeOffset>> _dateCachingService;
        private readonly ICachingService<(string cinema, string film, DateTimeOffset date), List<Booking>> _bookingCachingService;

        public BookingService(
            ICachingService<(string cinema, string film), List<DateTimeOffset>> dateCachingService,
            ICachingService<(string cinema, string film, DateTimeOffset date), List<Booking>> bookingCachingService,
            IApiClient apiClient)
        {
            _dateCachingService = dateCachingService;
            _bookingCachingService = bookingCachingService;

            _dateCachingService.Initialise(details => apiClient.GetDatesForFilmsByCinema(details.cinema, details.film));
            _bookingCachingService.Initialise(details => apiClient.GetBookings(details.cinema, details.film, details.date));
        }

        public Task<List<DateTimeOffset>> GetDates(string cinemaId, string filmId) 
            => _dateCachingService.Get((cinemaId, filmId));

        public Task<List<Booking>> GetBookings(string cinemaId, string filmId, DateTimeOffset screeningDate)
            => _bookingCachingService.Get((cinemaId, filmId, screeningDate));
    }
}