﻿namespace CineworldAlerter.Core.Services
{
    public interface ICineworldNavigationService
    {
        bool NavigateToMainPage(object parameter = null);
        void ClearBackStack();
        bool NavigateToSettingsPage(object parameter = null);
    }
}
