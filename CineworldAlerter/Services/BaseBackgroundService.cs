using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using CineworldAlerter.Core.Services;

namespace CineworldAlerter.Services
{
    public abstract class BaseBackgroundService<TTaskType> : IBackgroundService
    {
        public string BackgroundAgentName => $"{typeof(TTaskType).Name}";
        public string BackgroundAgentEntryPoint { get; } = typeof(TTaskType).FullName;

        public bool AgentRunning
        {
            get
            {
                var task = BackgroundTaskRegistration.AllTasks.FirstOrDefault(x => x.Value.Name == BackgroundAgentName);
                return task.Value != null;
            }
        }

        public bool CanRunTask
        {
            get
            {
                var status = BackgroundExecutionManager.GetAccessStatus();
                return status != BackgroundAccessStatus.DeniedByUser
                       && status != BackgroundAccessStatus.Unspecified
                       && status != BackgroundAccessStatus.DeniedBySystemPolicy;
            }
        }

        protected abstract IBackgroundTrigger GetTrigger();

        public void StopAgent()
        {
            if (!AgentRunning) return;

            BackgroundExecutionManager.RemoveAccess();
            var task = BackgroundTaskRegistration.AllTasks.FirstOrDefault(x => x.Value.Name == BackgroundAgentName);
            task.Value?.Unregister(true);
        }

        public async Task CreateAgent()
        {
            if (AgentRunning) return;

            if (CanRunTask)
            {
                CreateTask();
                return;
            }

            var status = await BackgroundExecutionManager.RequestAccessAsync();
            switch (status)
            {
                case BackgroundAccessStatus.AlwaysAllowed:
                case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                    CreateTask();
                    break;
                default:
                    return;
            }
        }

        private void CreateTask(bool includeEntryPoint = false)
        {
            var task = BackgroundTaskRegistration.AllTasks.FirstOrDefault(x => x.Value.Name == BackgroundAgentName);
            if (task.Value != null) return;

            try
            {
                var taskBuilder = new BackgroundTaskBuilder
                {
                    Name = BackgroundAgentName,
                };

                if (includeEntryPoint)
                    taskBuilder.TaskEntryPoint = BackgroundAgentEntryPoint;

                var trigger = GetTrigger();
                taskBuilder.SetTrigger(trigger);
                taskBuilder.Register();
            }
            catch
            {
                // If something has gone wrong here, not a lot we can do.
            }
        }
    }
}