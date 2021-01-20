
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public class EntityContainerDefault<T> : EntityContainer<T> where T : EntityReference, new()
    {
        List<T> myEntityContainerDefault = new List<T>();

        public override int Count { get { return myEntityContainerDefault.Count; } }

        public override bool IsReadOnly { get { return false; } }

        public override T this[int index]
        {
            get { return myEntityContainerDefault[index]; }
        }

        public EntityContainerDefault()
        {
        }

        private int Comparer(T A, T B)
        {

            return A.ID.CompareTo(B.ID);
        }


        protected override void Add(T item)
        {
           int Position = myEntityContainerDefault.BinarySearch(item);
            if(Position < 0)
            {
                myEntityContainerDefault.Add(item);
            }
        }

        public override void Clear()
        {
            myEntityContainerDefault.Clear();
        }

        public override bool Contains(T item)
        {
            return myEntityContainerDefault.Contains(item);
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            myEntityContainerDefault.CopyTo(array, arrayIndex);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return myEntityContainerDefault.GetEnumerator();
        }

        protected override bool Remove(T item)
        {
            var ItemToHandle = myEntityContainerDefault.Find(delegate (T Item)
            {
                return (Item.ID == item.ID);


            });
            if (ItemToHandle == null) return false;
            return myEntityContainerDefault.Remove(ItemToHandle);
        }

        public override void Sort()
        {
            myEntityContainerDefault.Sort();
        }
    }
}
