
using Engine.Components;
using Engine.Exceptions;
using Engine.Managers;
using System;
using System.Collections.Generic;

namespace Engine.Entities
{
    public class Entity
    {
        public World myWorldOwner;
        public EntityManager myEntityManager { get { return myWorldOwner.myEntityManager; } }
        public ComponentManager myComponentManager { get { return myWorldOwner.myComponentManager; } }
        public int ID { get; private set; }

        public Entity()
        {
            myWorldOwner = WorldManager.CurrentWorld;
            ID = ConcurrentUniqueID<Entity>.GetUID();
            myEntityManager.AddEntity(this);
        }

        public bool Destroyed
        {
            get
            {
                return myWorldOwner == null;
            }
        }
        public IReadOnlyList<Component> GetComponentsByTypeReadonly<T>() where T : Component, new()
        {
            return myComponentManager.GetComponentsByTypeReadonly<T>(this);
        }

        public List<Component> AddComponentsFromTypes(Type[] myComponents)
        {
            return myComponentManager.ForceAddComponent(this, myComponents);
        }

        public List<T> GetComponents<T>() where T : Component, new()
        {
            if (Destroyed)
                throw new InvalidEntityException();
            return myComponentManager.GetComponentsByType<T>(this);
        }

        public T GetComponent<T>() where T : Component, new()
        {
            if (Destroyed)
                throw new InvalidEntityException();
            return GetComponents<T>()?[0];
        }

        internal List<Component> AddComponentsFromTypes(Entity A, Type[] myTypes)
        {
            return myComponentManager.ForceAddComponent(this, myTypes);
        }

        public void RemoveComponent<T>(T myNewComponent) where T : Component
        {
            if (Destroyed) throw new InvalidEntityException();
            EntityBits Result = myEntityManager.GetEntityEntityBits(this);
            myComponentManager.RemoveComponent<T>(this, myNewComponent);
            myEntityManager.UpdateEntityEntityBits(this, Result);
        }


        public T AddComponent<T>() where T : Component, new()
        {
            if (Destroyed) throw new InvalidEntityException();
            T Component = null;
            EntityBits Result = myEntityManager?.GetEntityEntityBits(this);
            Component = myComponentManager?.AddComponent<T>(this);
            return Component;
        }

        public void Destroy()
        {
            myEntityManager.RemoveEntity(this);
        }

        ~Entity()
        {
            myEntityManager?.RemoveEntity(this);
            ConcurrentUniqueID<Entity>.FreeUID(ID);
        }

    }
}
