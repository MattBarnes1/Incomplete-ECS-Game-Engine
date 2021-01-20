using Engine.DataTypes;
using Engine.ECS.Scheduler.Threading;
using Engine.System_Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS.Systems.Default_Systems
{
    public class PhysicsRequestSystem : InterimSystems
    {
        public static ConcurrentQueue<PhysicsRequest> myRequests = new ConcurrentQueue<PhysicsRequest>();
        AABBTree myTree = new AABBTree();
        public static ConcurrentQueue<PhysicsUpdate> myUpdates = new ConcurrentQueue<PhysicsUpdate>();
        public override int JobCount { get { return myRequests.Count + myUpdates.Count; } }

        public PhysicsRequestSystem(World myWorld) : base(0)
        {





        }



        public override void RunAll(ThreadController Q)
        {

            //TODO: 

        }

        public override void Run()
        {
            if(myUpdates.Count != 0)
            {
                PhysicsUpdate Result;
                if(myUpdates.TryDequeue(out Result))
                {
                    Result.Update();
                    Result.Ready();
                }
            }
            else if(myRequests.Count != 0)
            {
                PhysicsRequest Result;
                if (myRequests.TryDequeue(out Result))
                {
                    Result.Perform();
                    Result.Ready();
                }
            }
        }
    }
}
