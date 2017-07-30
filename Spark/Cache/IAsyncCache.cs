using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spark.Cache
{
    public interface IAsyncCache<TKey, TValue>
    {
        Task AddAsync(TKey key, TValue value);
        ValueTask<uint> CountAsync();
        ValueTask<bool> ContainsAsync(TKey key);
        ValueTask<(bool exists, TValue value)> GetAsync(TKey key);
        ValueTask<ICollection<TKey>> GetKeysAsync();
        ValueTask<bool> IsEmptyAsync();
        ValueTask<bool> RemoveAsync(TKey key);
    }
}
