using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Engine.DataTypes
{
    public partial class Bag<T> : IEnumerable<T>, IReadOnlyList<T>
    {
        private T[] _items;
        private readonly bool _isPrimitive;
        private static int ExpandedCapacitySize;

        public int Capacity => _items.Length;
        public bool IsEmpty => Count == 0;
        public int Count { get; private set; }

        public Bag(int capacity = 16, int CapacitySizeIncrease = 1)
        {
            ExpandedCapacitySize = CapacitySizeIncrease;
            _isPrimitive = typeof(T).IsPrimitive;
            _items = new T[capacity];
        }

        public T this[int index]
        {
            get => index >= _items.Length ? default(T) : _items[index];
            set
            {
                EnsureCapacity(index + ExpandedCapacitySize);
                if (index >= Count)
                    Count = index + ExpandedCapacitySize;
                _items[index] = value;
            }
        }

        public void Add(T element)
        {
            EnsureCapacity(Count + ExpandedCapacitySize);
            _items[Count] = element;
            ++Count;
        }

        public void AddRange(IEnumerable<T> myEnum)
        {
            foreach(var A in myEnum)
            {
                Add(A);
            }
        }

        public void Clear()
        {
            if (Count == 0)
                return;

            // non-primitive types are cleared so the garbage collector can release them
            if (!_isPrimitive)
                Array.Clear(_items, 0, Count);

            Count = 0;
        }

        public bool Contains(T element)
        {
            for (var index = Count - 1; index >= 0; --index)
            {
                if (element.Equals(_items[index]))
                    return true;
            }

            return false;
        }

        public T RemoveAt(int index)
        {
            var result = _items[index];
            --Count;
            _items[index] = _items[Count];
            _items[Count] = default(T);
            return result;
        }

        public bool Remove(T element)
        {
            for (var index = Count - 1; index >= 0; --index)
            {
                if (element.Equals(_items[index]))
                {
                    --Count;
                    _items[index] = _items[Count];
                    _items[Count] = default(T);

                    return true;
                }
            }

            return false;
        }

        public bool RemoveAll(Bag<T> bag)
        {
            var isResult = false;

            for (var index = bag.Count - 1; index >= 0; --index)
            {
                if (Remove(bag[index]))
                    isResult = true;
            }

            return isResult;
        }

        private void EnsureCapacity(int capacity)
        {
            if (capacity < _items.Length)
                return;

            var newCapacity = capacity;
            var oldElements = _items;
            _items = new T[newCapacity];
            Array.Copy(oldElements, 0, _items, 0, oldElements.Length);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new BagEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BagEnumerator(this);
        }
    }
}
