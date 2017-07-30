using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spark.Cache
{
    public interface IAsyncCache<TKey, TValue>
    {
        Task Add(TKey key, TValue value);
        ValueTask<uint> CountAsync();
        ValueTask<bool> Contains(TKey key);
        ValueTask<(bool exists, TValue value)> Get(TKey key);
        ValueTask<bool> IsEmpty { get; }
        ValueTask<ICollection<TKey>> Keys { get; }
        ValueTask<bool> Remove(TKey key);
    }
}
