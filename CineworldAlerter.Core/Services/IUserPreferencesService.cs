using System;
using System.Collections.Generic;
using Cineworld.Api.Model;

namespace CineworldAlerter.Core.Services
{
    public interface IUserPreferencesService
    {
        event EventHandler UserPreferencesChanged;

        bool AlertOnEverything { get; set; }

        IEnumerable<FilmCategory> DontShowAlertsFor { get; set; }

        bool NotificationsFilterAppliesToList { get; set; }

        void Save();
    }
}
