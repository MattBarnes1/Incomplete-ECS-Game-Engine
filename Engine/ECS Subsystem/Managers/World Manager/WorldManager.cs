using System;
using System.Collections.Generic;
using Engine.Components;
using Engine.ECS_Subsystem.Managers;
using Engine.Entities;
using Engine.Resources;
using Engine.Systems;
using EngineRenderer;

namespace Engine.Managers
{
    [TestReminder("World Manager", "CreateWorld<T>")]
    public class WorldManager
    {
        private World InternalWorldHolder { get; set; }
        public static WorldManager myWorldManager { get; set; }
        public ArchetypeManager myArchetypeSystem { get; set; } //TODO: move this out of here, it doesn't make sense
        public static ResourceManager myCurrentResourceManager { get; set; }

        

        public void DefineArchetype(string v, Action<Component[]> myInitializer = null, params Type[] types)
        {
            myArchetypeSystem.DefineArchetype(v, myInitializer, types);
        }
        public void DefineArchetype(string v, params Type[] types)
        {
            myArchetypeSystem.DefineArchetype(v, null, types);
        }


        public Entity CreateEntityFromArchetype(string v, out List<Component> myCreatedComponents)
        {
            return myArchetypeSystem.CreateEntityFromArchetype(v, out myCreatedComponents);
        }


        public static ComponentManager myCurrentComponentManager
        {
            get
            {
                return myWorldManager?.InternalWorldHolder?.myComponentManager;
            }
        }
        public static EntityManager myCurrentEntityManager
        {
            get
            {
                return myWorldManager?.InternalWorldHolder?.myEntityManager;
            }
        }

        public static SystemScheduler myCurrentScheduler
        {
            get
            {
                return myWorldManager?.InternalWorldHolder?.mySystemScheduler;
            }
        }
        public LightingManager myLightManager;

        public RigidBodyManager myRigidBodyManager;

        public WorldManager(LightingManager LightManager = null, RigidBodyManager myManager = null)
        {
            myRigidBodyManager = myManager;
            myLightManager = LightManager;
            myCurrentResourceManager = new ResourceManager();
            myArchetypeSystem = new ArchetypeManager();
            myWorldManager = this;
        }


        public World ActiveWorld
        {
            get
            {
                return myWorldManager.InternalWorldHolder;
            }
        }

        public static World CurrentWorld
        {
            get
            {
                return myWorldManager.InternalWorldHolder;
            }
        }

        public void CreateNewDefaultWorld()
        {
            CreateWorld<World>();

        }

        public void CreateWorld<T>() where T : Engine.World
        {
            LoadWorldInstance((World)typeof(T).GetConstructor(new Type[] { }).Invoke(new Object[] { }));
        }

        public void LoadWorldInstance(World WorldFile)
        {
            if (InternalWorldHolder == null)
            {
                InternalWorldHolder = WorldFile; 
                if (myRigidBodyManager != null)
                {
                    InternalWorldHolder.myComponentManager.ComponentAddWatcher += myRigidBodyManager.ComponentAdded;
                    InternalWorldHolder.myComponentManager.ComponentRemoveWatcher += myRigidBodyManager.ComponentRemoved;
                }
                if (myLightManager != null)
                {
                    InternalWorldHolder.myComponentManager.ComponentAddWatcher += myLightManager.ComponentAdded;
                    InternalWorldHolder.myComponentManager.ComponentRemoveWatcher += myLightManager.ComponentRemoved;
                }
            }
            else
            {
                throw new Exception("NOT IMPLEMENTED");
            }
        }

        public World Load(String WorldFile)
        {
              return ResourceManager.Load<World>(WorldFile);
        }

        public void Unload()
        {
            InternalWorldHolder = null;
        }


        public void Update()
        {
            myCurrentScheduler.Run();
        }

        public void AddSystem<T>() where T : Systems.System
        {
            myCurrentScheduler.AddSystemToManager((Systems.System)typeof(T).GetConstructor(new Type[] { typeof(World) }).Invoke(new Object[] { InternalWorldHolder }));
        }
    }
}
