using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Resource_Manager.LRUCache
{
    public class LRUCache
    {
        //TOOD: ObjectPool<>
        List<LRUCacheObject> myCacheQueue = new List<LRUCacheObject>();
        public int MaxCacheSize { get; set; }
        public int EntryCount { get { return myObjects.Count; } }

        HashSet<Object> myObjects = new HashSet<object>();
        HashSet<String> myObjectID = new HashSet<String>();

        public LRUCache(int MaxCacheSize)
        {
            this.MaxCacheSize = MaxCacheSize;
        }

        public void Insert(object myObject, string myID)
        {
            if (String.IsNullOrEmpty(myID))
                throw new INVALID_CACHE_ID();
            if (myObject == null)
                throw new INVALID_CACHE_DATATYPE();
            if (myObjects.Contains(myObject))
                throw new DUPLICATE_CACHE_ITEM();
            if (myObjectID.Contains(myID))
                throw new INVALID_CACHE_ENTRY_DOUBLE_ID(myID);
            LRUCacheObject myLRUObject = new LRUCacheObject(myObject, myID);
            myObjects.Add(myObject);
            myObjectID.Add(myID);
            Enqueue(myLRUObject);
        }
        private LRUCacheObject DequeueAt(int Position)
        {
            var Object = myCacheQueue[Position];
            myCacheQueue.RemoveAt(Position);
            return Object;
        }
        private LRUCacheObject Dequeue()
        {
            return DequeueAt(myCacheQueue.Count - 1);

        }

        private void Enqueue(LRUCacheObject myLRUObject)
        {
            myCacheQueue.Insert(0, myLRUObject);
            if (myCacheQueue.Count() > MaxCacheSize)
            {
                var myDequeuedObject = Dequeue();
                myObjects.Remove(myDequeuedObject.Object);
                myObjectID.Remove(myDequeuedObject.ID);
            }
        }

        public T Get<T>(string v) where T: class
        {
            if (!myObjectID.Contains(v))
                return null;
            for(int i = 0; i < myCacheQueue.Count; i++)
            {
                LRUCacheObject myCacheItem = myCacheQueue[i];
                if (myCacheItem.ID.CompareTo(v) == 0)
                {
                    ObjectWasReused(i);
                    return myCacheItem.Object as T;
                }
            }
            return null;
        }

        private void ObjectWasReused(int Position)
        {
            var Objecta = DequeueAt(Position);
            Enqueue(Objecta);
        }
    }
}
