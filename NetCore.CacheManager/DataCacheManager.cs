using System;
using System.IO;
using System.Xml.Serialization;

namespace NetCore.CacheManager
{
    /// <summary>
    /// Gestor de caché 
    /// </summary>
    public class DataCacheManager : IDataCacheManager
    {
        private string _pathDirectoryCache = string.Empty;
        public string PathDirectoryCache
        {
            get
            {
                return _pathDirectoryCache;
            }
        }

        public DataCacheManager(string pathDirectoryCache)
        {
            _pathDirectoryCache = pathDirectoryCache;
        }

        private string GetFileNameCache(string identifier)
        {
            return _pathDirectoryCache + identifier + ".cache";
        }

        public T GetCache<T>(string identifier)
        {
            lock (this)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CacheFile<T>));
                string fileCache = GetFileNameCache(identifier);
                if (File.Exists(fileCache))
                {
                    CacheFile<T> dataCache = null;
                    var streamFile = File.OpenRead(fileCache);
                    using(streamFile)
                        using (StreamReader sr = new StreamReader(streamFile))
                            dataCache = ((CacheFile<T>)serializer.Deserialize(sr));

                    if (dataCache.DueDate.CompareTo(DateTime.Now) < 0)
                    {
                        File.Delete(fileCache);
                        dataCache = null;
                        return default(T);
                    }
                    else
                        return dataCache.Data;
                }
                return default(T);
            }
        }

        public void SaveCache<T>(string identifier, object data, int duration)
        {
            lock (this)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CacheFile<T>));
                string fileCache = GetFileNameCache(identifier);
                CacheFile<T> cache = new CacheFile<T>();
                cache.DueDate = DateTime.Now.AddMilliseconds(duration);
                cache.Data = (T)data;

                FileStream streamFile = null;
                if (File.Exists(fileCache))
                    streamFile = File.OpenWrite(fileCache);
                else
                    streamFile = File.Create(fileCache);
                
                using (streamFile)
                    using (StreamWriter writer = new StreamWriter(streamFile))
                        serializer.Serialize(writer, cache);
            }
        }

        public void Remove(string identifier)
        {
            string fileCache = GetFileNameCache(identifier);
            if (File.Exists(fileCache))
            {
                File.Delete(fileCache);
            }
        }
    }
}
