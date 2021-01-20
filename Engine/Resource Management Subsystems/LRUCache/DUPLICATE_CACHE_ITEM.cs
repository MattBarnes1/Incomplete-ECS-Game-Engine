using System;
using System.Runtime.Serialization;

namespace Engine.Resource_Manager.LRUCache
{
    [Serializable]
    internal class DUPLICATE_CACHE_ITEM : Exception
    {
        public DUPLICATE_CACHE_ITEM()
        {
        }

        public DUPLICATE_CACHE_ITEM(string message) : base(message)
        {
        }

        public DUPLICATE_CACHE_ITEM(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DUPLICATE_CACHE_ITEM(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}