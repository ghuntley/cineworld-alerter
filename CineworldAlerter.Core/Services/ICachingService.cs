using System;
using System.Threading.Tasks;

namespace CineworldAlerter.Core.Services
{
    public interface ICachingService<TValue>
    {
        bool IsInitialised { get; }

        void Initialise(Func<Task<TValue>> retrievalFunc);

        Task<TValue> Get();

        void ClearCache();
    }
}
