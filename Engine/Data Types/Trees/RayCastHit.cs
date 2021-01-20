using Engine.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine.DataTypes
{
    public class RayCastHit<T> : IEnumerator<T>
    {
        private List<T> list;
        int Counter = -1;
        public RayCastHit(List<T> list)
        {
            this.list = list;
        }
        public T this[int index]
        {
            get
            {
               return list[index];
            }
        }
        public int Count { get { return list.Count; } }

        public T Current { get
            {
                if (list.Count != 0)
                    return list[Counter];
                else
                    throw new Exception("Invalid Attempt at Iterating Raycast");
            }
        } 

        object IEnumerator.Current { 
            get {
                if (list.Count != 0)
                    return list[Counter];
                else
                    throw new Exception("Invalid Attempt at Iterating Raycast");
            } }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            if (list.Count > Counter)
            {
                Counter++;
                return true;
            }
            else
                return false;
        }

        public void Reset()
        {
            Counter = -1;
        }
    }
}