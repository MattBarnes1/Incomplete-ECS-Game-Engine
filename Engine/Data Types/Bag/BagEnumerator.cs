using System.Collections.Generic;
using System.Collections;

namespace Engine.DataTypes
{
    public partial class Bag<T>
    {
        internal struct BagEnumerator : IEnumerator<T>
            {
                private volatile Bag<T> _bag;
                private volatile int _index;

                public BagEnumerator(Bag<T> bag)
                {
                    _bag = bag;
                    _index = -1;
                }

                T IEnumerator<T>.Current => _bag[_index];
                object IEnumerator.Current => _bag[_index];

                public bool MoveNext()
                {
                    return ++_index < _bag.Count;
                }

                public void Dispose()
                {
                }

                public void Reset()
                {
                }
            }
        }
    }
