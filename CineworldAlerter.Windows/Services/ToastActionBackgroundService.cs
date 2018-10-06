using Windows.ApplicationModel.Background;
using CineworldAlerter.Windows.BackgroundTasks;

namespace CineworldAlerter.Services
{
    public class ToastActionBackgroundService : BaseBackgroundService<ToastActionsBackgroundTask>
    {
        protected override IBackgroundTrigger GetTrigger() 
            => new ToastNotificationActionTrigger();

        protected override bool UseEntryPoint { get; } = true;
    }
}