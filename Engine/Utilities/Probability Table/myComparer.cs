using System.Collections.Generic;

namespace Engine.Utilities
{
    internal class ProbabilityComparer : IComparer<ProbabilityEntry>
    {
        float myValue;
        public ProbabilityComparer(float valueToCompare)
        {
            myValue = valueToCompare;
        }
        public int Compare(ProbabilityEntry x, ProbabilityEntry y)
        {
            ProbabilityEntry myCompare = null;
            if (x == null) myCompare = y;
            if (y == null) myCompare = x;
            return myCompare.myProbability.CompareTo(myValue);
        }
    }
}