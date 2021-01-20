using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Resource_Manager.LRUCache
{
    public class LRUCacheObject
    {
        public String ID { get; set; }
        public object Object { get; set; }
        public LRUCacheObject(int v) { }


        public LRUCacheObject()
        {
        }

        public LRUCacheObject(object Object, String ID)
        {
            this.ID = ID;
            this.Object = Object;
        }
    }




}
