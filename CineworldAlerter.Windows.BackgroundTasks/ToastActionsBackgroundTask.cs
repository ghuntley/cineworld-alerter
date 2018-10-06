using System;
using Windows.ApplicationModel.Background;
using Windows.System;
using Windows.UI.Notifications;
using CineworldAlerter.Windows.Core.Services;

namespace CineworldAlerter.Windows.BackgroundTasks
{
    public sealed class ToastActionsBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (!(taskInstance.TriggerDetails is ToastNotificationActionTriggerDetail triggerDetails))
                return;

            var deferral = taskInstance.GetDeferral();

            try
            {
                var argument = triggerDetails.Argument;

                if (string.IsNullOrEmpty(argument) || !argument.StartsWith(ToastService.LauncherCode))
                    return;

                if (argument.StartsWith(ToastService.LauncherCode))
                {
                    HandleLauncher(argument);
                    return;
                }
            }
            catch (Exception ex)
            {
                // Something
            }
            finally
            {
                deferral.Complete();
            }
        }

        private void HandleLauncher(string argument)
        {
            var url = argument.Replace(ToastService.LauncherCode, string.Empty);
            Launcher.LaunchUriAsync(new Uri(url, UriKind.RelativeOrAbsolute)).AsTask();
        }
    }
}
