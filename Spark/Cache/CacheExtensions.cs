using System.Linq;
using System.Threading.Tasks;

namespace Spark.Cache
{
    public static class CacheExtensions
    {
        public static void Clear<TKey, TValue>(this ICache<TKey, TValue> cache)
        {
            foreach (var key in cache.Keys)
            {
                cache.Remove(key);
            }
        }

        public static async Task Clear<TKey, TValue>(this IAsyncCache<TKey, TValue> cache)
        {
            await Task.WhenAll(from key in await cache.Keys select cache.Remove(key).AsTask());
        }
    }
}
