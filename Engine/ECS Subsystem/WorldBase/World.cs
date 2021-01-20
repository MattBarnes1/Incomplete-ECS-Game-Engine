using Engine.Default_Systems;
using Engine.ECS.Systems.Default_Systems;
using Engine.ECS.Systems.Default_Systems.Input;
using Engine.Entities;
using Engine.Managers;
using Engine.Systems;
using System;
using System.Diagnostics;

namespace Engine
{
    public class World
    {
       
         public readonly String WorldID;
        public ComponentManager myComponentManager { get; private set; }
        public EntityManager myEntityManager { get; private set; }
        public SystemScheduler mySystemScheduler { get; private set; }

        public World() : base()
        {
            myComponentManager = new ComponentManager(this);
            myEntityManager = new EntityManager(this);
            myComponentManager.ComponentAddWatcher += myEntityManager.UpdateEntityEntityBits;
            myComponentManager.ComponentRemoveWatcher += myEntityManager.UpdateEntityEntityBits;
            mySystemScheduler = new SystemScheduler(this);
            AddSystem<DrawSystem>();
            AddSystem<WindowInputSystem>();
            AddSystem<MouseInputSystem>();
            AddSystem<KeyboardInputSystem>();
            AddSystem<DragHandlerSystem>();
            AddSystem<TimerSystem>();
           // AddSystem<CursorSystem>();
            AddSystem<SoundFXSystem>();
            AddSystem<PhysicsRequestSystem>();
            AddSystem<CameraControllerSystem>();
            //Entity Declarations
        }



        public void AddSystem<T>()
        {
            try
            {
                mySystemScheduler.AddSystemToManager((Engine.Systems.System)typeof(T).GetConstructor(new Type[] { typeof(World) }).Invoke(new Object[] { this }));
            }
            catch (Exception E)
            {
                Debug.WriteLine("System: " + typeof(T) + " has failed to intialize.");
            }
        }

        public void Update()
        {
            mySystemScheduler.Run();
        }

        ~World()
        {
        }


    }
}
