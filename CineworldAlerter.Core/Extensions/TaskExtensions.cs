using System.Threading.Tasks;

namespace CineworldAlerter.Core.Extensions
{
    public static class TaskExtensions
    {
        public static void DontAwait(this Task task) { }
    }
}
