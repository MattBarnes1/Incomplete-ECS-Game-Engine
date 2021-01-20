using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public static class ConcurrentUniqueID<ClassRequestingID>
    {
        static ConcurrentStack<int> UsedNumbers = new ConcurrentStack<int>();
        static int lastNumber;
        static object LockLastNumber = new object();        

        public static int GetUID()
        {
            if(UsedNumbers.TryPop(out var Result))
            {
                return Result;
            }
            else
            {
                lock (LockLastNumber)
                {
                    return lastNumber++;
                }
            }
        }

        public static void FreeUID(int ID)
        {
            UsedNumbers.Push(ID);
        }
    }
}
