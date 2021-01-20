using Engine.ECS.Scheduler.SystemSchedulerTree;
using Engine.ECS.Scheduler.Threading;
using Engine.System_Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
namespace Engine.Systems
{
    public class SystemScheduler
    {
        static SystemScheduler ActiveManager;
        SchedulerTree mySystemsSchedulerTree = new SchedulerTree();
        private World world;
        ThreadController mYController = new ThreadController();
        public SystemScheduler(World world)
        {
            if (ActiveManager == null)
                ActiveManager = this;
            this.world = world;
        }
        

      
        public void AddSystemToManager(Engine.Systems.System mySystem)
        {
            if (mySystem.GetType().IsSubclassOf(typeof(InterimSystems)))
            {
                AddInterimSystem((InterimSystems)mySystem);
            }
            mySystemsSchedulerTree.AddSystem(mySystem);
            myTheads = mySystemsSchedulerTree.RebuildThreadingSystem();
        }
        List<InterimSystems> myInterimSystems = new List<InterimSystems>();
        private void AddInterimSystem(InterimSystems mySystem)
        {
            myInterimSystems.Add(mySystem);
        }

        Task myTheads;


        bool FirstRun;

        public void Run()
        {
#if DEBUG
            if(!FirstRun)
            {
                for (int i = 0; i < mySystemsSchedulerTree.RowHeight; i++)
                {
                    Debug.WriteLine("Row: " + i);
                    foreach (Engine.Systems.System A in mySystemsSchedulerTree.GetSystemRow(i))
                    {
                        Debug.WriteLine(A.GetType().ToString());
                    }
                    Debug.WriteLine("");
                }
                FirstRun = true;
            }
#endif


            for(int i = 0; i < mySystemsSchedulerTree.RowHeight; i++)
            {
                foreach(Engine.Systems.System A in mySystemsSchedulerTree.GetSystemRow(i))
                {
                    A.Preprocess();
                    A.Run(mYController);
                }//Starts running our threads
                while(!mYController.isIdle())
                {
                    mYController.HandItemsToThreadsLoop(); //sets up next thread controll count.                    
                    int g = 0;
                    while (!mYController.threadsNeedMoreWork() && g < myInterimSystems.Count)
                    {
                        if (myInterimSystems[g].JobCount != 0)
                            myInterimSystems[g].Run(mYController);
                        else
                            g++;                                
                    }
                }
                foreach (Engine.Systems.System A in mySystemsSchedulerTree.GetSystemRow(i))
                {
                    A.Postprocess();
                }
            }
        }
    }
}
