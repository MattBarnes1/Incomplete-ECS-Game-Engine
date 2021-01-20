using Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers
{
    public class EntityManager : Manager
    {


        public Action<Entity> EntityAddWatcher;
        public Action<Entity> EntityRemoveWatcher;
        public EntityManager(World world) : base(world)
        {
        }

        private readonly Dictionary<Entity, EntityBits> EntityToEntityBits = new Dictionary<Entity, EntityBits>();


        internal Dictionary<Entity, EntityBits> GetEntities()
        {
            return EntityToEntityBits;
        }


 
        internal EntityBits GetEntityEntityBits(Entity ID)
        {
            return EntityToEntityBits[ID];
        }


        internal void UpdateEntityEntityBits(Entity ID, EntityBits result)
        {
            EntityToEntityBits[ID] = result;
        }




        public void RemoveEntity(Entity entity)
        {
            EntityBits returned = null;
            EntityToEntityBits.TryGetValue(entity, out returned);
            myBase.myEntityManager.EntityRemoveWatcher?.Invoke(entity);
            if(returned != null)
            {
                EntityToEntityBits.Remove(entity);
                entity.myWorldOwner = null;
            }
        }

        public void AddEntity(Entity entity)
        {
            EntityToEntityBits.Add(entity, new EntityBits());
        }

        public int GetEntityCount()
        {
            return EntityToEntityBits.Count();
        }
    }
}
