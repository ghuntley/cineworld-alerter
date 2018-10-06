using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cineworld.Api;
using Cineworld.Api.Model;
using Moq;

namespace CineworldAlerter.Core.Tests.Mocks
{
    public class ApiClientMock : MockBase<IApiClient>
    {
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
