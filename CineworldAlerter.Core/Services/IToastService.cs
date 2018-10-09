﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Cineworld.Api.Model;

namespace CineworldAlerter.Core.Services
{
    public interface IToastService
    {
        Task DisplayToasts(IEnumerable<FullFilm> films);
        Task AnnounceUnlimitedScreenings(IEnumerable<FullFilm> films);
        bool CanFilmBeDisplayed(FullFilm film);
    }

    public interface IToastProxyService
    {
        void ShowToast(FullFilm film, bool isUnlimitedScreening);
        void AnnounceUnlimitedScreening(FullFilm film);
    }
}
