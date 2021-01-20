





using Engine.Components;
using Engine.Components.Base;
using Engine.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Engine.Entities
{
    public abstract class EntityIterator<T> where T : EntityReference, new()
    {
        protected World myWorld { get; private set; }
        Matcher InternalMatcher;
        public EntityIterator(World myBase, EntityContainer<T> myCustomContainer)
        {
            myWorld = myBase;
            LoadGenericTypesIntoMatcher();
            myEntitySet = myCustomContainer;
            RegisterWithEntityAndComponentManagers();
        }


        public EntityIterator(World myBase)
        {
            myWorld = myBase;
            LoadGenericTypesIntoMatcher();
            RegisterWithEntityAndComponentManagers();
        }

        private void LoadGenericTypesIntoMatcher()
        {
            Type[] myType = typeof(T).GetGenericArguments();
            InternalMatcher = new Matcher();
            foreach (Type A in myType)
            {
                InternalMatcher.AddToAndSet(ComponentRegistration.GetComponentBitset(A));
            }
        }

        internal bool isDirty()
        {
            throw new NotImplementedException();
        }

        protected EntityContainer<T> GetContainer()
        {
            return myEntitySet;
        }

        internal bool Matches(EntityBits componentBits)
        {
            throw new NotImplementedException();
        }

        private void RegisterWithEntityAndComponentManagers()
        {
            myWorld.myEntityManager.EntityRemoveWatcher += this.EntityRemoved;
            myWorld.myComponentManager.ComponentAddWatcher += this.ProcessEntityChange;
            myWorld.myComponentManager.ComponentRemoveWatcher += this.ProcessEntityChange;
        }

        private void EntityRemoved(Entity arg1)
        {
            EntityDeleted.Enqueue(arg1);
        }

        ~EntityIterator()
        {
            UnregisterEntityAndComponentManagers();
        }

        private void UnregisterEntityAndComponentManagers()
        {
            myWorld.myEntityManager.EntityRemoveWatcher -= this.EntityRemoved;
            myWorld.myComponentManager.ComponentAddWatcher -= this.ProcessEntityChange;            
            myWorld.myComponentManager.ComponentRemoveWatcher -= this.ProcessEntityChange;
        }

        private void EntityComponentRemoved(Entity arg1, EntityBits arg2)
        {
            ComponentChanged.Enqueue(new Tuple<Entity, EntityBits>(arg1, arg2));
        }

        private void EntityComponentAdded(Entity arg1, EntityBits arg2)
        {
            ComponentChanged.Enqueue(new Tuple<Entity, EntityBits>(arg1, arg2));
        }
        ConcurrentQueue<Entity> EntityDeleted = new ConcurrentQueue<Entity>();

        ConcurrentQueue<Tuple<Entity, EntityBits>> ComponentChanged = new ConcurrentQueue<Tuple<Entity, EntityBits>>();

        protected virtual void ProcessEntityChange(Entity myEntity, EntityBits ComponentBits)
        {
            if (InternalMatcher.Matches(ComponentBits))
            {
                myEntitySet.Add(myEntity);
            }
            else
            {
                myEntitySet.Remove(myEntity);
            }
        }


        public void UpdateChanges()
        {
            while (ComponentChanged.Count > 0)
            {
                if (ComponentChanged.TryDequeue(out var Result))
                {
                    ProcessEntityChange(Result.Item1, Result.Item2);
                }
            }
            myEntitySet.Sort();
        }

        EntityContainer<T> myEntitySet = new EntityContainerDefault<T>();

      
        int Position = -1;
        public virtual bool hasNext()
        {
            Position++;
            if (Position < Count)
            {
                return true;
            }
            else
            {
                Position = -1;
                return false;
            }
        }

        public virtual T Current
        {
            get
            {
                if (Position < 0) return null;
                return myEntitySet[Position];
            }
        }

        public int Count { get { return myEntitySet.Count; } }
    }
}
