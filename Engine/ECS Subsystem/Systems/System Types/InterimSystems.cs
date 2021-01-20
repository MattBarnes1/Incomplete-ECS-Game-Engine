using Engine.ECS.Scheduler.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System_Types
{
    public abstract class InterimSystems : Engine.Systems.System
    {

        public abstract int JobCount { get; }

        public InterimSystems(int SystemLevel) : base(SystemLevel)
        {
        }

        public override void Postprocess()
        {

        }

        protected override void Process(ThreadController myController)
        {
            while (JobCount != 0)
            {
                if(myController.threadsNeedMoreWork())
                {
                    if(myController.hasWaitingJobs())
                    {
                        myController.HandItemsToThreadsLoop();
                    } 
                    else
                    {
                        //Substitute Jobs we're waiting to finish.
                        RunAll(myController);
                        myController.HandItemsToThreadsLoop();
                    }
                }
                else
                {
                    Run();
                }
            }
        }

        public abstract void Run();

        public abstract void RunAll(ThreadController Q);

        public override void Preprocess()
        {

        }

    }
}
