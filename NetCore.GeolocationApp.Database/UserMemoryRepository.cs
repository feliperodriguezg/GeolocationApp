using NetCore.CacheManager;
using NetCore.GeolocationApp.Repositories.Interfaces;
using System;

namespace NetCore.GeolocationApp.Repositories
{

    public class UserMemoryRepository : IUsersRepository
    {
        private IDataCacheManager _cacheManager;
        public UserMemoryRepository(IDataCacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public bool Authenticate(string username, string password)
        {
            bool result = true;
            var cache = _cacheManager.GetCache<InternalUserData>(username);
            if (cache == null)
                result = false;
            else
            {
                result = cache.Username == username && cache.Password == password;
            }
            return result;
        }

        public bool BlockUser(object userIdentifier, bool block)
        {
            bool result = true;
            string username = (string)userIdentifier;
            var cache = _cacheManager.GetCache<InternalUserData>(username);

            if (cache == null)
                result = false;
            else
            {
                cache.IsBlocked = block;
                _cacheManager.SaveCache<InternalUserData>(username, cache, 1000000);
            }
            return result;
        }

        public bool DeleteUser(object userIdentifier)
        {
            string username = (string)userIdentifier;
            var cache = _cacheManager.GetCache<InternalUserData>(username);
            if (cache != null)
                _cacheManager.Remove(username);
            return true;
        }

        public UserData GetUserData(object userIdentifier)
        {
            string username = (string)userIdentifier;
            var cache = _cacheManager.GetCache<InternalUserData>(username);
            return cache;
        }

        public bool RegisterNewUser(string passwordUser, UserData userData)
        {
            bool result = true;
            if (String.IsNullOrEmpty(userData.Username) || String.IsNullOrEmpty(passwordUser))
                result = false;
            {
                var cache = _cacheManager.GetCache<InternalUserData>(userData.Username);
                if (cache == null)
                {
                    InternalUserData internalUserData = new InternalUserData(userData);
                    internalUserData.Password = passwordUser;
                    _cacheManager.SaveCache<InternalUserData>(userData.Username, internalUserData, 1000000);
                    result = true;
                }
                else
                    result = false;
            }
            return result;
        }

        public bool UpdateUserInformation(object userIdentifier, UserData userData)
        {
            bool result = true;
            string username = (string)userIdentifier;
            if(String.IsNullOrEmpty(username) || String.IsNullOrEmpty(userData.Username))
            {
                result = false;
            }
            else
            {
                var cache = _cacheManager.GetCache<InternalUserData>(username);
                if (cache == null)
                    result = false;
                else
                {
                    UserData data = (UserData)cache;
                    data.Email = cache.Email;
                    data.Name = cache.Name;
                    _cacheManager.Remove(username);
                    _cacheManager.SaveCache<InternalUserData>(username, cache, 1000000);
                    result = true;
                }
            }
            return result;
        }
    }
}
