using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public static class UUID<T>
    {
        static Stack<int> UsedNumbers = new Stack<int>();
        static int lastNumber;
        public static int GetUID()
        {
            if(UsedNumbers.Count != 0)
            {
                return UsedNumbers.Pop();
            }
            else
            {
                return lastNumber++;
            }
        }

        public static void FreeUID(int ID)
        {
            UsedNumbers.Push(ID);
        }



    }
}
