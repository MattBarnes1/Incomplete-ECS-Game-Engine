
using Engine.Entities;
using Engine.ECS.Scheduler.Threading;


using System;
using System.Collections.Generic;

namespace Engine.System_Types
{
    /*public abstract class OneToManySystem<SingleTypes, ManyTypes> : Engine.Systems.SystemBase where SingleTypes : EntityReference, new() where ManyTypes : EntityReference, new()
    {
        SingleTypes myReferenceSingleton;
        ManyTypes myReferencePolymorphic;
        EntityIterator<ManyTypes> myIteratorSingleEntities;
        EntityIterator<SingleTypes> myIteratorManyEntities;
        public OneToManySystem(int SystemLevel) : base(SystemLevel)
        {
            myIteratorSingleEntities = new EntityIterator<ManyTypes>();
            myIteratorManyEntities = new EntityIterator<SingleTypes>();
        }

        protected abstract void Process(SingleTypes myReferenceSingle, ManyTypes myReferencedMany);


        protected override void Process(ThreadController myController)
        {
            myIteratorSingleEntities.UpdateChanges();
            myIteratorManyEntities.UpdateChanges();
            myIterator.UpdateChanges();
            while (myIteratorSingleEntities)
                while (myIterator.hasNext())
                {
                    if (myController != null)
                    {
                        SingleTypes mySavedReferenced = myReferenceSingleton;
                        ManyTypes myReferencedPolyMorphic = myIteratorManyEntities.Current;
                        myController.AddDelegate(delegate (Object A)
                        {
                            Process(mySavedReferenced, myReferencedPolyMorphic);
                        });
                    }
                    else
                    {
                        Process(myReferenceSingleton, myIteratorManyEntities.Current);
                    }
                }

        }
    }*/
    public abstract class ManyToManySystem<ManyLeft, ManyRight> : Engine.Systems.System where ManyLeft:EntityReference, new() where ManyRight : EntityReference, new()
    {
        EntityIterator<ManyLeft> myIteratorMany;
        EntityIterator<ManyRight> myIteratorOtherMany;
        EntityContainer<ManyRight> myIteratorManyOther;

        public ManyToManySystem(World myWorld, int SystemLevel = 3) : base(SystemLevel)
        {
            myIteratorOtherMany = new EntityIteratorDefault<ManyRight>(myWorld);
            myIteratorMany = new EntityIteratorDefault<ManyLeft>(myWorld);
        }

        protected abstract void Process(ManyLeft myFirstLeft, EntityContainer<ManyRight> myIteratorTwo);

        protected sealed override void Process(ThreadController myController)
        {
            while(myIteratorMany.hasNext())
            {
                if (myController != null)
                {
                    ManyLeft CurrentInstance = myIteratorMany.Current;
                    myController.AddDelegate(delegate (Object A)
                    {
                       // Process(CurrentInstance, myIteratorOtherMany); //TODO: Fix this
                    });
                }
                else
                {
                  //  Process(myIteratorMany.Current, myIteratorOtherMany);
                }
            }
        }
    }
}
