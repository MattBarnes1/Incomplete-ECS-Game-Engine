using System;

namespace Engine.Resource_Manager.LRUCache
{

    [System.Serializable]
    public class INVALID_CACHE_ID : System.Exception
    {
        public INVALID_CACHE_ID() { }
        public INVALID_CACHE_ID(string message) : base(message) { }
    }


    [System.Serializable]
    public class INVALID_CACHE_CONVERSION : System.Exception
    {
        public INVALID_CACHE_CONVERSION() { }
        public INVALID_CACHE_CONVERSION(string message) : base(message) { }
    }


    [System.Serializable]
    public class INVALID_CACHE_DATATYPE : System.Exception
    {
        public INVALID_CACHE_DATATYPE() { }
        public INVALID_CACHE_DATATYPE(string message) : base(message) { }

    }


    [System.Serializable]
    public class INVALID_CACHE_ENTRY_DOUBLE_ID : System.Exception
    {
        public INVALID_CACHE_ENTRY_DOUBLE_ID(String ID) : base("Double ID Entry: " + ID + " was used!") { }

    }

}