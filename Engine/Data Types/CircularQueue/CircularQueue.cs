using System;
using System.Collections;
using System.Collections.Generic;
using Vulkan;

namespace Engine.Data_Types
{
    internal class CircularQueue<T> : IEnumerable<T>
    {
        int lastSetInQueue = 0;
        T[] myQueue;
        uint lastUsed = 0;
        public CircularQueue(int QueueSize)
        {
            myQueue = new T[QueueSize];
        }

        public uint Count { get { return (uint)lastSetInQueue; } }

        object locker = new object();

        public void Insert(T myItem)
        {
            myQueue[lastSetInQueue] = myItem;
            lastSetInQueue++;
        }

        public IEnumerator GetEnumerator()
        {
            return myQueue.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
           return myQueue.GetEnumerator() as IEnumerator<T>;
        }

        public T GetNext(out uint Position)
        {
            lock (locker)
            {
                if (lastUsed == lastSetInQueue)
                {
                    lastUsed = 0;
                }
                Position = lastUsed;
                return myQueue[lastUsed++];
            }
        }

        public void Reset()
        {
            lastUsed = 0;
        }
    }
}