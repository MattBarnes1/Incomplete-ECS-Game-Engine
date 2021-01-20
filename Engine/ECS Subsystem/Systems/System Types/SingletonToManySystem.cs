using System;

using Engine.Entities;
using Engine.ECS.Scheduler.Threading;
using Engine.ECS.Components.Base;
using Engine.Components;

namespace Engine.System_Types
{
    public abstract class SingletonToManySystem<T> : Engine.Systems.System where T : EntityReference, new()
    {
        EntityIterator<T> myIterator;
        public SingletonToManySystem(World myWorld, int SystemLevel = 1) : base(SystemLevel)
        {
            myIterator = new EntityIteratorDefault<T>(myWorld);
        }

        protected abstract void Process(T myReferenceSingle);

        public virtual void CustomIteratorUpdate()
        { }

        protected override void Process(ThreadController myController)
        {
            myIterator.UpdateChanges();

            while (myIterator.hasNext())
            {
                if (myController != null)
                {
                    T mySavedReferenced = myIterator.Current;
                    myController.AddDelegate(delegate (Object A)
                    {
                        Process(mySavedReferenced);
                    });
                }
                else
                {
                    Process(myIterator.Current);
                }
            }
        }
    }

    public abstract class SingletonToManySystem<U, T> : Engine.Systems.System where U: Component where T : EntityReference, new()
    {
        EntityIterator<T> myIterator;
        protected U SingletonInstance;
        bool customSystem;
        public SingletonToManySystem(World myWorld, int SystemLevel) : base(SystemLevel)
        {
            SingletonInstance = SingletonComponent<U>.Get();
            myIterator = new EntityIteratorDefault<T>(myWorld);
        }
        public SingletonToManySystem(World myWorld, EntityIterator<T> CustomSystem, int SystemLevel) : base(SystemLevel)
        {
            SingletonInstance = SingletonComponent<U>.Get();
            myIterator = CustomSystem;
        }

        protected abstract void Process(T myReferenceSingle);


        protected override void Process(ThreadController myController)
        {

            myIterator.UpdateChanges();
            while (myIterator.hasNext())
            {
                if (myController != null)
                {
                    T mySavedReferenced = myIterator.Current;
                    myController.AddDelegate(delegate (Object A)
                    {
                        Process(mySavedReferenced);
                    });
                }
                else
                {
                    Process(myIterator.Current);
                }
            }
        }

        protected EntityIterator<T> GetEntityIterator()
        {
            return myIterator;
        }
    }
}
