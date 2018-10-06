using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CineworldAlerter.Core.Services
{
    public class CachingService<TKey, TValue> : ICachingService<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Task<TValue>> _retrievalTasks = new ConcurrentDictionary<TKey, Task<TValue>>();

        private Func<TKey, Task<TValue>> _retrievalFunc;

        public bool IsInitialised { get; private set; }

        public void Initialise(Func<TKey, Task<TValue>> retrievalFunc)
        {
            if(IsInitialised)
                throw new InvalidOperationException();

            _retrievalFunc = retrievalFunc;

            IsInitialised = true;
        }

        public async Task<TValue> Get(TKey key)
        {
            ThrowIfNotInitialised();

            return await _retrievalTasks.GetOrAdd(key, x => _retrievalFunc(x));
        }

        public void ClearCache()
            => _retrievalTasks.Clear();

        public void ClearCache(TKey key) 
            => _retrievalTasks.TryRemove(key, out _);

        private void ThrowIfNotInitialised()
        {
            if (!IsInitialised)
                throw new InvalidOperationException($"Cache is not initialised via {nameof(Initialise)}, cannot call {nameof(Get)}.");
        }
    }

    public class CachingService<TValue> : ICachingService<TValue>
    {
        private readonly ICachingService<bool, TValue> _cachingService;

        public bool IsInitialised => _cachingService.IsInitialised;

        public CachingService(
            ICachingService<bool, TValue> cachingService)
            => _cachingService = cachingService;

        public void Initialise(Func<Task<TValue>> retrievalFunc)
            => _cachingService.Initialise(_ => retrievalFunc());

        public Task<TValue> Get() 
            => _cachingService.Get(true);

        public void ClearCache()
            => _cachingService.ClearCache();
    }
}