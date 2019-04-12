using System.Linq;
using Cimbalino.Toolkit.Services;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Extensions;

namespace CineworldAlerter.ViewModels.Entities
{
    public class BookingViewModel
    {
        private readonly ILauncherService _launcherService;

        private Booking _booking;

        public string BookingTime => _booking.BookingTime.TimeOfDay.ToString(@"hh\:mm");

        public bool IsSoldOut => _booking.IsSoldOut;

        public bool Is3D => _booking.AttributeIds.Contains("3D") || _booking.AttributeIds.Contains("3d");

        public bool IsSubtitled => _booking.AttributeIds?.Contains("subbed") ?? false;

        public BookingViewModel(
            ILauncherService launcherService) 
            => _launcherService = launcherService;

        public void LaunchBooking()
            => _launcherService.LaunchUriAsync(_booking.BookingLink);

        public BookingViewModel WithBooking(Booking booking)
        {
            _booking = booking;
            return this;
        }
    }
}