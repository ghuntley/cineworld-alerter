using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cineworld.Api;
using Cineworld.Api.Model;
using Moq;

namespace CineworldAlerter.Core.Tests.Mocks
{
    public class ApiClientMock
    {
        private Mock<IApiClient> _mock;

        public Mock<IApiClient> Mock => _mock ?? (_mock = new Mock<IApiClient>());

        public IApiClient Object => Mock.Object;

        public ApiClientMock WithGetFilmsForCinema(IEnumerable<CinemaFilm> films)
        {
            Mock
                .Setup(x => x.GetFilmsForCinema(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(films.ToList());

            return this;
        }

        public ApiClientMock WithGetAllFilms(IEnumerable<FullFilm> films)
        {
            Mock
                .Setup(x => x.GetAllFilms(It.IsAny<CancellationToken>()))
                .ReturnsAsync(films.ToList());

            return this;
        }

        public ApiClientMock WithGetCinemas(IEnumerable<Cinema> cinemas)
        {
            Mock
                .Setup(x => x.GetCinemas(It.IsAny<CancellationToken>()))
                .ReturnsAsync(cinemas.ToList());

            return this;
        }
    }
}
