using System;
using Engine.ECS.Components.Base;
using Engine.ECS.Scheduler.Threading;
using Engine.Entities;

namespace Engine.System_Types
{
    public abstract class OneToManySystem<T, U> : Engine.Systems.System where T: SingletonComponent<T>, new() where U:EntityReference, new()
    {
        public T myReferenceSingleton { get; }
        EntityIterator<U> myIterator;
        public OneToManySystem(World myBase, int SystemLevel = 1) : base(SystemLevel)
        {
            myReferenceSingleton = SingletonComponent<T>.Get();
            myIterator = new EntityIteratorDefault<U>(myBase);
        }




        protected abstract void Process(T myReferenceSingle, U myReferencedMany);


        protected override void Process(ThreadController myController)
        {
            myIterator.UpdateChanges();
            while (myIterator.hasNext())
            {
                if (myController != null)
                {
                    T mySavedReferenced = myReferenceSingleton;
                    U myReferencedPolyMorphic = myIterator.Current;
                    myController.AddDelegate(delegate (Object A)
                    {
                        Process(mySavedReferenced, myReferencedPolyMorphic);
                    });
                }
                else
                {
                    Process(myReferenceSingleton, myIterator.Current);
                }
            }

        }
    }
}
