namespace NetCore.CacheManager
{
    public interface IDataCacheManager
    {
        string PathDirectoryCache { get; }

        T GetCache<T>(string identifier);
        void Remove(string identifier);
        void SaveCache<T>(string identifier, object data, int duration);
    }
}