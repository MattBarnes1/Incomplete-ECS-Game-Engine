using System;

using Engine.ECS.Scheduler.Threading;
using Engine.Entities;

namespace Engine.System_Types
{
    public abstract class OneToOneSystem<T, U> : Engine.Systems.System where T : EntityReference where U: EntityReference
    {
        T myReference;
        U myReferenceOther;

        public OneToOneSystem(World myBase, int SystemLevel = 1) : base(SystemLevel)
        {//TODO:

        }

        protected abstract void Process(T mySingleReference, U myOtherReferenced);


        protected sealed override void Process(ThreadController myController)
        {
            if(myController != null)
            {
                myController.AddDelegate(delegate (Object A)
                {
                    Process(myReference, myReferenceOther);
                });
            }
            else
            {
                Process(myReference, myReferenceOther);
            }
        }
    }
}
