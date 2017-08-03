using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NetCore.CacheManager
{
    [XmlRoot("Root")]
    public class CacheFile<T>
    {
        [XmlElement]
        public DateTime DueDate { get; set; }
        [XmlElement]
        public T Data { get; set; }
    }
}
