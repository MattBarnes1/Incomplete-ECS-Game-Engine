
using Engine.Entities;
using Engine.System_Types;
using Engine.Systems;
using System;

namespace Engine.Default_Systems
{
    [SystemReader(typeof(TimerComponent), typeof(GameTimeUpdateComponent))]
    [SystemWriter(typeof(TimerComponent))]
    public class TimerSystem : SingletonToManySystem<GameTimeUpdateComponent, EntityReference<TimerComponent>>
    {
        public TimerSystem(World MyBase) : base(MyBase, 0)
        {

        }

        public override void Postprocess()
        {

        }

        public override void Preprocess()
        {

        }


        protected override void Process(EntityReference<TimerComponent> myReferencedMany)
        {
            foreach (var A in myReferencedMany.Component1)
            {
                if (!A.isPaused)
                    A.timerUpdate(SingletonInstance.ElapsedTime);
            }
        }


    }
}
