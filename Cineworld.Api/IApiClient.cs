﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cineworld.Api.Model;

namespace Cineworld.Api
{
    public interface IApiClient
    {
        Task<List<Cinema>> GetCinemas(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<CinemaFilm>> GetFilmsForCinema(string cinemaId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<FullFilm>> GetAllFilms(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<FullFilm>> SearchUnlimitedFilms(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<DateTimeOffset>> GetDatesForFilmsByCinema(string cinemaName, string filmId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Booking>> GetBookings(string cinemaName, string filmId, DateTimeOffset screeningDate, CancellationToken cancellationToken = default(CancellationToken));
    }
}