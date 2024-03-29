﻿using Windows.ApplicationModel.Background;
using CineworldAlerter.Windows.BackgroundTasks;

namespace CineworldAlerter.Services
{
    public class RefreshBackgroundService : BaseBackgroundService<TimedRefreshBackgroundTask>
    {
        protected override IBackgroundTrigger GetTrigger()
            => new TimeTrigger(30, false);

        protected override bool UseEntryPoint { get; } = true;
    }
}