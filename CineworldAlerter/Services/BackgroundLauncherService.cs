using System.Linq;
using System.Threading.Tasks;
using CineworldAlerter.Core.Services;

namespace CineworldAlerter.Services
{
    public class BackgroundLauncherService : IBackgroundLauncherService
    {
        private readonly IBackgroundService[] _backgroundTaskServices =
        {
            new RefreshBackgroundService()
        };

        public Task Startup() 
            => Task.WhenAll(_backgroundTaskServices.Select(x => x.CreateAgent()));
    }
}
