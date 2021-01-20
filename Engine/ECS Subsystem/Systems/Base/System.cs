

using Engine.ECS.Scheduler.Threading;
using Engine.Entities;
using Engine.Managers;
using System.Reflection;

namespace Engine.Systems
{
    public abstract class System
    {
        Matcher SystemReader = new Matcher();
        Matcher SystemWriter = new Matcher();
        public int ExecutionOrder { get; private set; }
        public System(int SystemLevel)
        {
           var Result = GetType().GetCustomAttribute<SystemReader>();
            if(Result != null)
            {
                foreach (var B in Result.ComponentType)
                {
                    EntityBits myReturnedComponentID = ComponentRegistration.GetComponentBitset(B);
                    SystemReader.AddToAndSet(myReturnedComponentID);
                }
            }
            var Result2 = GetType().GetCustomAttribute<SystemWriter>();
            if (Result2 != null)
            {
                foreach (var B in Result2.ComponentType)
                {
                    EntityBits myReturnedComponentID = ComponentRegistration.GetComponentBitset(B);
                    SystemWriter.AddToAndSet(myReturnedComponentID);
                }
            }

            this.ExecutionOrder = SystemLevel;
        }
        public abstract void Preprocess();
        public abstract void Postprocess();
        protected abstract void Process(ThreadController myController);

        protected bool SystemToggle = true;

        public void Run(ThreadController myController)
        {
            if(SystemToggle)
            {
                Process(myController);
            }
        }

        public void ToggleOff()
        {
            SystemToggle = false;
        }
        public void ToggleOn()
        {
            SystemToggle = true;
        }

        public Matcher GetReaderEntityBits()
        {
            return SystemReader;
        }
        public Matcher GetWriterEntityBits()
        {
            return SystemWriter;
        }
    }
}
