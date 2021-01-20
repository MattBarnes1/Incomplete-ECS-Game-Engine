
using Engine.Managers;
using System;

namespace Engine.Entities
{
    public class Matcher
    {
        Lazy<ComponentArray> myOrArray = new Lazy<ComponentArray>(delegate()
        {
            return new ComponentArray();
        });
        Lazy<ComponentArray> myAndArray = new Lazy<ComponentArray>(delegate ()
        {
            return new ComponentArray();
        });
        Lazy<ComponentArray> myNotArray = new Lazy<ComponentArray>(delegate ()
        {
            return new ComponentArray();
        }); //TODO: optimize checks

        public Matcher()
        {
        }

        public bool Matches(EntityBits myOtherArray)
        {
            bool ORResult = false;
            bool ANDResult = false;
            bool NOTResult = true;
            bool HasAtLeastOne = false;
            if (myOrArray.IsValueCreated)
            {
                EntityBits OrArray = myOrArray.Value.Get();
                ORResult = OrArray.OrTest(myOtherArray);
                HasAtLeastOne = true;
            }
            if (myAndArray.IsValueCreated)
            {
                EntityBits AndArray = myAndArray.Value.Get();
                ANDResult = AndArray.AndTest(myOtherArray);
                HasAtLeastOne = true;
            }
            if (myNotArray.IsValueCreated)
            {
                EntityBits NotArray = myNotArray.Value.Get(); //TODO: Cache Get()?
                NOTResult = !NotArray.AndTest(myOtherArray);
                HasAtLeastOne = true;
            }
            return ((ANDResult || ORResult) && NOTResult);
        }

      
        public void AddToNotSet(EntityBits myArray2)
        {
            myNotArray.Value.Add(myArray2);
            if (hasConflict(myAndArray, myNotArray) || hasConflict(myOrArray, myNotArray))
            {
                throw new Exception("And Array and Not Array conflicted!");
            }
        }

        public void AddToOrSet(EntityBits myArray2)
        {
            myOrArray.Value.Add(myArray2);
            if (hasConflict(myOrArray, myNotArray))
            {
                throw new Exception("Or Array and Not Array conflicted!");
            }
        }

        public void AddToAndSet(EntityBits myArray2)
        {
            myAndArray.Value.Add(myArray2);
            if (hasConflict(myAndArray, myNotArray))
            {
                throw new Exception("And Array and Not Array conflicted!");
            }
        }

        private bool hasConflict(Lazy<ComponentArray> myAndOrArray, Lazy<ComponentArray> myNotArray)
        {
            if(!myAndOrArray.IsValueCreated || !myNotArray.IsValueCreated)
            {
                return false;
            }
            return myNotArray.Value.Get().AndTest(myAndOrArray.Value.Get());
        }
        public EntityBits GetAndBits()
        {
            return myAndArray.Value.Get();
        }
        public EntityBits GetOrBits()
        {
            return myOrArray.Value.Get();
        }
    }
}
