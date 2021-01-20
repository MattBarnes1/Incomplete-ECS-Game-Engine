#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine.Utilities
{
    
    public class ProbabilityEntry 
    {
        
        public float myProbability;
        
        object myObject;
        
        int Amount = 1;
        public void getItem(object Item, ref int amountOf)
        {
            Item = myObject;
            amountOf = Amount;
        }

        public void SetItem(object myNewMonster)
        {
            myObject = myNewMonster;
        }
    }

}
