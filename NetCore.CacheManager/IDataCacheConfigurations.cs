using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.CacheManager
{
    public interface IDataCacheConfigurations
    {
        string PathDirectory { get; set; }
    }
}
