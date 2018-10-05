using System.Threading.Tasks;

namespace CineworldAlerter.Core.Services
{
    public interface IBackgroundLauncherService
    {
        Task Startup();
    }

    public interface IBackgroundService
    {
        bool AgentRunning { get; }
        bool CanRunTask { get; }
        void StopAgent();
        Task CreateAgent();
    }
}
