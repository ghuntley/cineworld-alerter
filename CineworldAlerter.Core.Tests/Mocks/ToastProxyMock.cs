using Cineworld.Api.Model;
using CineworldAlerter.Core.Services;
using Moq;

namespace CineworldAlerter.Core.Tests.Mocks
{
    public class ToastProxyMock : MockBase<IToastProxyService>
    {
        public ToastProxyMock()
            : base(MockBehavior.Strict) { }

        public ToastProxyMock WithFilm(FullFilm film, bool isUnlimitedScreening)
        {
            Mock
                .Setup(x => x.ShowToast(
                    It.Is<FullFilm>(y => y.Code == film.Code),
                    It.Is<bool>(y => y == isUnlimitedScreening)));

            return this;
        }
    }
}