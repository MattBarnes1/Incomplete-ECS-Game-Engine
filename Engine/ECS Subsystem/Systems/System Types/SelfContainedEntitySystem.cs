using System;
using Engine.ECS.Scheduler.Threading;
using Engine.Entities;

namespace Engine.System_Types
{


    public abstract class SelfContainedEntitySystem<T> : Engine.Systems.System where T:EntityReference, new()
    {
        EntityIterator<T> myIterator;
        public SelfContainedEntitySystem(World myWorld, int SystemLevel) : base(SystemLevel)
        {
            myIterator = new EntityIteratorDefault<T>(myWorld);
        }

        protected abstract void Process(T myEntitySet);

        protected sealed override void Process(ThreadController myController)
        {
            myIterator.UpdateChanges();
            while (myIterator.hasNext())
            {
                if (myController != null)
                {
                    T myEntitySetSave = myIterator.Current;
                    myController.AddDelegate(delegate (Object A)
                    {
                        Process(myEntitySetSave);
                    });
                }
                else
                {
                    Process(myIterator.Current);
                }
            }
        }

    }
}
