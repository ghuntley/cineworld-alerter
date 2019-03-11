using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services;
using CineworldAlerter.Core.Services.Local;
using CineworldAlerter.Core.Tests.Mocks;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace CineworldAlerter.Core.Tests.Services
{
    public class FilmServiceTests
    {
        private readonly ApiClientMock _apiMock = new ApiClientMock();
        private readonly Mock<ICachingService<string, List<FullFilm>>> _cachingMock = new Mock<ICachingService<string, List<FullFilm>>>();
        private readonly Mock<ILocalStorageService> _storageMock = new Mock<ILocalStorageService>();
        private readonly Mock<IToastService> _toastMock = new Mock<IToastService>();

        private Func<string, Task<List<FullFilm>>> _retrievalFunc;

        #region Film Items

        private readonly FullFilm _jurassicParkFull = new FullFilm
        {
            Code = "1",
            FeatureTitle = "Jurassic Park"
        };

        private readonly FullFilm _lostWorldFull = new FullFilm
        {
            Code = "2",
            FeatureTitle = "The Lost World"
        };

        private readonly FullFilm _jurassicWorldFull = new FullFilm
        {
            Code = "3",
            FeatureTitle = "Jurassic World"
        };

        private readonly CinemaFilm _jurassicParkLite = new CinemaFilm
        {
            FilmId = "1",
            FilmName = "Jurassic Park"
        };

        private readonly CinemaFilm _jurassicWorldLite = new CinemaFilm
        {
            FilmId = "3",
            FilmName = "Jurassic World"
        };
        #endregion

        public FilmServiceTests()
            => _cachingMock
                .Setup(x => x.Initialise(It.IsAny<Func<string, Task<List<FullFilm>>>>()))
                .Callback<Func<string, Task<List<FullFilm>>>>(x => _retrievalFunc = x);

        [Fact]
        public async Task GetLocalFilms_NoFile_ReturnsNoFilms()
        {
            _storageMock
                .Setup(x => x.FileExistsAsync(It.Is<string>(y => y == FilmService.FilmCacheFile)))
                .ReturnsAsync(false);

            _cachingMock
                .Setup(x => x.Get(It.Is<string>(y => y == FilmService.FilmCacheFile)))
                .Returns(() => _retrievalFunc(FilmService.FilmCacheFile));

            var target = GetTarget();

            var actual = await target.GetLocalFilms();
            Assert.Empty(actual);
        }

        [Fact]
        public async Task RefreshFilms_AddsAndRemovesCorrectly()
        {
            _toastMock
                .Setup(x => x.DisplayToasts(It.IsAny<IEnumerable<FullFilm>>()))
                .Returns(Task.CompletedTask);

            _cachingMock
                .Setup(x => x.Get(It.Is<string>(y => y == FilmService.FilmCacheFile)))
                .ReturnsAsync(new List<FullFilm>
                {
                    _jurassicParkFull,
                    _lostWorldFull
                });

            _apiMock
                .WithGetFilmsForCinema(new []{_jurassicParkLite, _jurassicWorldLite})
                .WithGetAllFilms(new [] {_jurassicParkFull, _jurassicWorldFull});

            var target = GetTarget();
            await target.RefreshFilms("8112");

            _toastMock
                .Verify(x => x.DisplayToasts(
                    It.Is<IEnumerable<FullFilm>>(y =>
                        y.First() == _jurassicWorldFull)), Times.Once);

            _storageMock
                .Verify(x => x.WriteAllTextAsync(
                    It.Is<string>(y => y == FilmService.FilmCacheFile),
                    JsonConvert.SerializeObject(new List<FullFilm>
                    {
                        _jurassicParkFull,
                        _jurassicWorldFull
                    })), Times.Once);

            _storageMock
                .Verify(x => x.WriteAllTextAsync(
                    It.Is<string>(y => y == FilmService.AllFilmCacheFile),
                    JsonConvert.SerializeObject(new List<FullFilm>
                    {
                        _jurassicParkFull,
                        _jurassicWorldFull
                    })), Times.Once);
        }

        private FilmService GetTarget()
            => new FilmService(
                _apiMock.Object,
                _cachingMock.Object,
                _storageMock.Object,
                _toastMock.Object);
    }
}
