#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine.Utilities
{
    
    public class ProbabilityTable : ProbabilityEntry
    {
        
        private List<ProbabilityEntry> myProbabilities;
        public void OnEnable()
        {
            if(myProbabilities == null)
                myProbabilities = new List<ProbabilityEntry>();
        }

        private void RollForTable(object myReturned,ref int myAmount)
        {
           float myRange = Random.Range(0, 100);
           ProbabilityEntry myEntryToUse = FindTableByFloat(myRange);
           myEntryToUse.getItem(myReturned, ref myAmount);
        }

        internal new void getItem(object Item, ref int amountOf)
        {
            RollAgainstTable(ref Item, ref amountOf);
        }


        private ProbabilityEntry FindTableByFloat(float myRange)
        {
            int location = myProbabilities.BinarySearch(null, new ProbabilityComparer(myRange));
            return myProbabilities[location];
        }

        private void RollAgainstTable(ref object myItems, ref int myQuantity)
        {
            RollForTable(myItems,ref myQuantity);
        }

        public void RollAgainstTable(int TimesToRoll, ref  List<object> myItems, ref List<int> myQuantities)
        {
            for(int i= 0; i< TimesToRoll; i++)
            {
                object myObjectRef = null;
                int myIntRef = 0;
                RollAgainstTable(ref myObjectRef, ref myIntRef);
                myItems.Add(myObjectRef);
                myQuantities.Add(myIntRef);
            }
        }

        public void AddToTable(object myNewMonster, float spawnProbability)
        {//TODO: Add check for random
            //Debug.Assert(spawnProbability > 0 && spawnProbability < 100, "Odd spawn probability detected!");
            ProbabilityEntry myEntry = new ProbabilityEntry();
            myEntry.myProbability = spawnProbability;
            myEntry.SetItem(myNewMonster);
            myProbabilities.Add(myEntry);
        }
        public void AddTable(ProbabilityTable AddNewTable)
        {
            myProbabilities.Add(AddNewTable);
        }

    }

}
