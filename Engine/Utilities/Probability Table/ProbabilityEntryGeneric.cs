#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine.Utilities
{
    
    public class ProbabilityEntry<U> : ProbabilityEntry
    {
        U myObject;
        int Quantity;

      

        public void Set(U myValue)
        {
            myObject = myValue;
        }

        internal new void getItem(ref object Item, ref int amountOf)
        {
            Item = myObject;
            amountOf = Quantity;
        }
    }


}
