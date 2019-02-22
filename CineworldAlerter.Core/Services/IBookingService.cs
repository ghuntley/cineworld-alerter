using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cineworld.Api.Extensions;
using Cineworld.Api.Model;

namespace CineworldAlerter.Core.Services
{
    public interface IBookingService
    {
        Task<List<DateTimeOffset>> GetDates(string cinemaId, string filmId);
        Task<List<Booking>> GetBookings(string cinemaId, string filmId, DateTimeOffset screeningDate);
    }
}
