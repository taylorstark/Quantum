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
    }
}
