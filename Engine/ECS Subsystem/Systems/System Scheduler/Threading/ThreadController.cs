using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Management.Instrumentation;
using System.Management;
using System.Diagnostics;

namespace Engine.ECS.Scheduler.Threading
{
    public class ThreadController
    {
        readonly int MaxThreadsToUse;
        ConcurrentStack<Action<Object>> myDelegates = new ConcurrentStack<Action<Object>>();
        int MaxThreads;
        public ThreadController()
        {
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get()) //TODO: Won't work on linux
            {
                MaxThreads += int.Parse(item["NumberOfLogicalProcessors"].ToString());
            }
            myEvent = new CountdownEvent(0);
        }
        CountdownEvent myEvent;
        public void AddDelegate(Action<Object> myDelegate)
        {
            myDelegates.Push(myDelegate);         
        }

        private void ResetCountdown()
        {
            myEvent = new CountdownEvent(0);
        }
        public void HandItemsToThreadsLoop()
        {
            if (myDelegates.Count != 0 && myEvent.CurrentCount != Math.Min(myDelegates.Count, MaxThreads))
            {
                if (myEvent.IsSet)
                {
                    myEvent = new CountdownEvent(Math.Min(myDelegates.Count, MaxThreads));
                    LoadCurrentDelegates();
                }
                else if (myEvent.TryAddCount(Math.Min(myDelegates.Count, MaxThreads))) //will return false if is set
                {
                    LoadCurrentDelegates();
                }
            }
        }

        public bool threadsNeedMoreWork()
        {
            return (myEvent.CurrentCount != Math.Min(myDelegates.Count, MaxThreads));
        }
        public bool hasWaitingJobs()
        {
            return (myDelegates.Count != 0);
        }
        public bool isIdle()
        {
            return (myDelegates.Count == 0) && myEvent.CurrentCount == 0;

        }

        private void LoadCurrentDelegates()
        {
            int Count = Math.Min(myDelegates.Count, MaxThreads);
            for (int i = 0; i < Count; i++)
            {
                Action<object> myResult = null;
                myDelegates.TryPop(out myResult);
                if (myResult != null)
                {
                    ThreadPool.QueueUserWorkItem(delegate (object obj)
                    {
                        try
                        {
                            myResult(obj);
                        }
                        catch (Exception E)
                        {
                            Debug.WriteLine(E);
                        }
                        myEvent.Signal();
                    });
                } else
                {
                    i--;
                }
            }
        }
    }
}
