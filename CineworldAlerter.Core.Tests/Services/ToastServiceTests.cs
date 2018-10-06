using System.Collections.Generic;
using System.Threading.Tasks;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services;
using CineworldAlerter.Core.Tests.Mocks;
using Moq;
using Xunit;

namespace CineworldAlerter.Core.Tests.Services
{
    public class ToastServiceTests
    {
        private readonly UserPreferencesMock _userPreferencesMock = new UserPreferencesMock();
        private readonly ToastProxyMock _toastProxyMock = new ToastProxyMock();

        private readonly FullFilm _jurassicParkFull = new FullFilm
        {
            Code = "1",
            FeatureTitle = "Jurassic Park",
            Attributes = new List<FilmCategory>
            {
                FilmCategory.ThreeD,
                FilmCategory.Pg,
                FilmCategory.UnlimitedScreening
            }
        };

        private readonly FullFilm _lostWorldFull = new FullFilm
        {
            Code = "2",
            FeatureTitle = "The Lost World",
            Attributes = new List<FilmCategory>
            {
                FilmCategory.Eighteen,
                FilmCategory.TwoD,
                FilmCategory.AudioDescribed
            }
        };

        [Fact]
        public async Task DisplayToasts_ShowsAllToasts()
        {
            _userPreferencesMock
                .WithShowEverything(true)
                .WithDontShowAlertsFor(null);

            _toastProxyMock
                .WithFilm(_jurassicParkFull, true)
                .WithFilm(_lostWorldFull, false);

            var target = GetTarget();
            await target.DisplayToasts(new[] {_jurassicParkFull, _lostWorldFull});

            _toastProxyMock
                .Mock
                .Verify(x => x.ShowToast(It.IsAny<FullFilm>(), It.IsAny<bool>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DisplayToasts_ShowsRestrictionToasts()
        {
            _userPreferencesMock
                .WithShowEverything(false)
                .WithDontShowAlertsFor(new[] {FilmCategory.Eighteen});

            _toastProxyMock
                .WithFilm(_jurassicParkFull, true);

            var target = GetTarget();
            await target.DisplayToasts(new[] {_jurassicParkFull, _lostWorldFull});
        }

        [Fact]
        public async Task DisplayToasts_DontShowEverythingNoExceptions_ShowsAllToasts()
        {
            _userPreferencesMock
                .WithShowEverything(false)
                .WithDontShowAlertsFor(null);

            _toastProxyMock
                .WithFilm(_jurassicParkFull, true)
                .WithFilm(_lostWorldFull, false);

            var target = GetTarget();
            await target.DisplayToasts(new[] { _jurassicParkFull, _lostWorldFull });

            _toastProxyMock
                .Mock
                .Verify(x => x.ShowToast(It.IsAny<FullFilm>(), It.IsAny<bool>()), Times.Exactly(2));
        }

        private ToastService GetTarget()
            => new ToastService(
                _userPreferencesMock.Object,
                _toastProxyMock.Object);
    }
}