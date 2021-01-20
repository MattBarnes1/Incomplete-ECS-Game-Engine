using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data_Types.LockedList
{
    public class ConcurrentList<T>
    {
        List<T> myLockedList = new List<T>();
        object myLock = new object(); 
        public void AddItem(T myItem)
        {
            lock(myLock)
            {
                myLockedList.Add(myItem);
            }
        }

        public void RemoveItem(T myItem)
        {
            lock (myLock)
            {
                myLockedList.Remove(myItem);
            }
        }
        public T[] ToArray(T myItem)
        {
            lock (myLock)
            {
                return myLockedList.ToArray();
            }
        }
    }
}
