using System.Collections.Generic;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services;
using Moq;

namespace CineworldAlerter.Core.Tests.Mocks
{
    public class UserPreferencesMock : MockBase<IUserPreferencesService>
    {
        public UserPreferencesMock()
            : base(MockBehavior.Strict)
        {
        }

        public UserPreferencesMock WithShowEverything(bool showEverything)
        {
            Mock
                .SetupGet(x => x.AlertOnEverything)
                .Returns(showEverything);

            return this;
        }

        public UserPreferencesMock WithDontShowAlertsFor(IEnumerable<FilmCategory> items)
        {
            Mock
                .SetupGet(x => x.DontShowAlertsFor)
                .Returns(items);

            return this;
        }
    }
}