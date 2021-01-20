using Engine.Components;
using Engine.ECS.Components.Base;
using Engine.ECS.Scheduler.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS.Systems.System_Types
{
    public abstract class SingletonComponentUpdateSystem<T> : Engine.Systems.System where T:Component
    {
        protected T myComponent = SingletonComponent<T>.Get();
        protected T GetSingleton()
        {
            return myComponent;
        }
        public SingletonComponentUpdateSystem(World BaseWorld) : base(0)
        {
        }
    }
}
