using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data_Types.BufferPool
{
    [TestReminder("BufferPool<T>", "Command buffer requests exceeds next.")]
    public abstract class BufferPool<T>
    {
        protected List<T> myTakenBuffers = new List<T>();
        protected List<T> myFreeBuffers = new List<T>();

        object myLock = new object();

        public T GetNext()
        {
            lock (myLock)
            {
                if (myFreeBuffers.Count != 0)
                {
                    var Value = myFreeBuffers[0];
                    myFreeBuffers.RemoveAt(0);
                    myTakenBuffers.Add(Value);
                    return Value;
                }
                else
                {
                    AllocateBuffers(10);
                    var Value = myFreeBuffers[0];
                    myFreeBuffers.RemoveAt(0);
                    myTakenBuffers.Add(Value);
                    return Value;
                }
            }
        }

        protected abstract void AllocateBuffers(int v);

        public int TotalRemaining()
        {
            return myFreeBuffers.Count();
        }

        internal int TotalTakenCount()
        {
            return myTakenBuffers.Count();
        }

        internal T[] ActivelyUsedBuffersToArray()
        {
            return myTakenBuffers.ToArray();
        }

        public virtual void SwapQueueResetBehaviour(T A)
        {

        }
        internal virtual void Rebuild()
        {

        }
        public void ResetQueues()
        {
            if (myTakenBuffers.Count == 0) return;
            foreach(T A in myTakenBuffers)
            {
                SwapQueueResetBehaviour(A);
            }
            myFreeBuffers.AddRange(myTakenBuffers);
            myTakenBuffers.Clear();
        }
        //TODO: Does one time submit reset this and make the swapque function useless.
    }
}
