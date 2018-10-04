using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CineworldAlerter.Core.Services
{
    public class CachingService<TValue> : ICachingService<TValue>
    {
        private readonly ConcurrentDictionary<bool, Task<TValue>> _retrievalTasks = new ConcurrentDictionary<bool, Task<TValue>>();

        private Func<Task<TValue>> _retrievalFunc;

        public bool IsInitialised { get; private set; }

        public void Initialise(Func<Task<TValue>> retrievalFunc)
        {
            if(IsInitialised)
                throw new InvalidOperationException();

            _retrievalFunc = retrievalFunc;

            IsInitialised = true;
        }

        public async Task<TValue> Get()
        {
            ThrowIfNotInitialised();

            return await _retrievalTasks.GetOrAdd(true, x => _retrievalFunc());
        }

        public void ClearCache()
            => _retrievalTasks.Clear();

        private void ThrowIfNotInitialised()
        {
            if (!IsInitialised)
                throw new InvalidOperationException($"Cache is not initialised via {nameof(Initialise)}, cannot call {nameof(Get)}.");
        }
    }
}