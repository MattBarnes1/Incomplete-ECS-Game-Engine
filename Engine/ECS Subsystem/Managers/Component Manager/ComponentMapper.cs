using Engine.DataTypes;
using Engine.Entities;
using System;
using System.Linq;

namespace Engine.Managers
{

    public abstract class ComponentMapper
    {
        protected ComponentMapper(EntityBits id, Type componentType)
        {
            Id = id;
            ComponentType = componentType;
        }

        public EntityBits Id { get; }
        public Type ComponentType { get; }
        public abstract bool Has(int entityId);
        public abstract void Delete(int entityId);

        public abstract object Get(int entityID);
    }

    public class ComponentMapper<T> : ComponentMapper
        where T : class
    {

        private readonly Action<int> _onCompositionChanged;

        public ComponentMapper(EntityBits id, Action<int> onCompositionChanged)
            : base(id, typeof(T))
        {
            _onCompositionChanged = onCompositionChanged;
            Components = new Bag<Bag<T>>();
        }

        public Bag<Bag<T>> Components { get; }

        public void Put(int entityId, T component)
        {
            if (Components[entityId] == null)
            {
                Components[entityId] = new Bag<T>()
                {
                    component
                };         
            } 
            else
            {
                Components[entityId].Add(component);
            }
            //TODO: should we alert or not? -> Nothing's changed so it's going to recalculate the same
            _onCompositionChanged?.Invoke(entityId);
        }

        public Bag<T> Get(Entity entity)
        {
            return Get(entity.ID) as Bag<T>;
        }

        public override object Get(int entityId)
        {
            return Components[entityId];
        }

        public override bool Has(int entityId)
        {
            if (entityId >= Components.Count())
                return false;

            return Components[entityId] != null;
        }

        public override void Delete(int entityId)
        {
            Components[entityId] = null;
            _onCompositionChanged?.Invoke(entityId);
        }
    }
}
