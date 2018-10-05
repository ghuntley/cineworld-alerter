using Windows.ApplicationModel.Background;
using CineworldAlerter.Background;

namespace CineworldAlerter.Services
{
    public class RefreshBackgroundService : BaseBackgroundService<TimedRefreshBackgroundTask>
    {
        protected override IBackgroundTrigger GetTrigger()
            => new TimeTrigger(30, false);
    }
}