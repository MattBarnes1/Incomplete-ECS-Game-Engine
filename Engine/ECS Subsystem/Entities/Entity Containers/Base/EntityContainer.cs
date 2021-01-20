using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine.Entities
{
    public abstract class EntityContainer<T> : IEnumerable<T> where T : EntityReference, new()
    {
        public abstract int Count { get; }


        public abstract bool IsReadOnly { get; }

        public void Add(Entity A)
        {
            T myNewEntity = new T();
            myNewEntity.CopyFrom(A);
            Add(myNewEntity);
        }

        protected abstract void Add(T item);
        public abstract void Clear();
        public abstract T this[int index] { get; }
        public abstract bool Contains(T item);
        public abstract void CopyTo(T[] array, int arrayIndex);

        public abstract IEnumerator<T> GetEnumerator();
        public bool Remove(Entity item)
        {
            T myNewEntity = new T();
            myNewEntity.CopyFrom(item);
            return Remove(myNewEntity);
        }
        protected abstract bool Remove(T item);
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract void Sort();
    }
}
