
using System;
using System.Collections.Generic;




using System.Reflection;
using System.Linq;
using Engine.Entities;
using Engine.DataTypes;
using Engine.Components;

namespace Engine.Managers
{
    public class ComponentManager : Manager
    {
        public Action<Entity, EntityBits> ComponentAddWatcher;
        public Action<Entity, EntityBits> ComponentRemoveWatcher;
        public ComponentManager(World world) : base(world)
        {
            myMethod = this.GetType().GetRuntimeMethods().Where(delegate (MethodInfo I)
            {
                if (I.Name == "AddComponent")
                {
                    if (I.GetGenericArguments().Count() == 1)
                    {
                        return true;
                    }
                }
                return false;
            }).First();
            myBase = world;
            _componentMappers = new Dictionary<EntityBits, ComponentMapper>();
        }
        internal void RemoveComponent<T>(Entity entity, T myNewComponent) where T : Component
        {
            var Map = GetMapper<T>();
            Bag<T> Bag = Map.Get(entity);
            Bag.Remove(myNewComponent);
        }
        MethodInfo myMethod;
        internal List<Component> ForceAddComponent(Entity ID, Type[] myComponents)
        {
            List<Component> retComponents = new List<Component>();
            foreach (Type A in myComponents)
            {
                retComponents.Add((Component)myMethod.MakeGenericMethod(A).Invoke(this, new Object[] { ID }));

            }
            return retComponents;
        }
        //DO NOT CHANGE IT WILL BREAK THE LOOKUP FUNCTION FOR THE ARCHETYPE SYSTEM
        internal T AddComponent<T>(Entity entity) where T: Component, new()
        {
            T item = new T();
            ComponentMapper<T> myMap = GetMapper<T>();
            myMap.Put(entity.ID, item);
            
            var entityOriginalArray = entity.myEntityManager.GetEntityEntityBits(entity);
            entityOriginalArray = entityOriginalArray | ComponentRegistration.GetComponentBitset(typeof(T));
            ComponentAddWatcher?.Invoke(entity, entityOriginalArray);
            return item;
        }

        public IReadOnlyList<T> GetComponentsByTypeReadonly<T>(Entity myEntity) where T : Component, new()
        {
            var Map = GetMapper<T>();
            var Bag = Map.Get(myEntity);
            return (IReadOnlyList<T>)Bag;
        }
        public List<T> GetComponentsByType<T>(Entity myEntity) where T : Component, new()
        {
            var Map = GetMapper<T>();
            var Bag = Map.Get(myEntity);
            var ListReturned = new List<T>();
            int Count = 0;
            if (Bag == null) return null;
            foreach(var A in Bag)
            {
                if(A != null)
                    ListReturned.Add(A);
                Count++;
                if (Count == Bag.Count()) break;
            }
            return ListReturned;
        }



        private readonly Dictionary<EntityBits, ComponentMapper> _componentMappers;

        
        internal void RemoveEntity(Entity entity)
        {
            foreach (var componentMapper in _componentMappers)
                componentMapper.Value.Delete(entity.ID);
        }

      

        private ComponentMapper<T> CreateMapperForType<T>(EntityBits componentTypeId)
            where T : class
        {
            if(typeof(T) == typeof(Component))
            {
                throw new Exception("Mapper using Component Base Class was created!");
            }
            var mapper = new ComponentMapper<T>(componentTypeId, ComponentsChanged);
            _componentMappers[componentTypeId] = mapper;
            return mapper;
        }

        private void ComponentsChanged(int obj)
        {
        }

        public ComponentMapper GetMapper(EntityBits componentTypeId)
        {
            return _componentMappers[componentTypeId];
        }

        public ComponentMapper<T> GetMapper<T>()
            where T : class
        {
            if (typeof(T) == typeof(Component))
            {
                throw new Exception("Mapper using only Component Base Class was almost retrieved!");
            }
            var componentTypeId = ComponentRegistration.GetComponentBitset(typeof(T));

            if (_componentMappers.TryGetValue(componentTypeId, out var Result))
                return Result as ComponentMapper<T>;

            return CreateMapperForType<T>(componentTypeId);
        }


        public EntityBits CreateComponentBits(int entityId)
        {
            var componentBits = new EntityBits();
            int bit = 0;
            foreach(var T in _componentMappers)
            {
                if(T.Value.Has(entityId))
                {
                    componentBits = componentBits | (T.Key);
                } 
            }
            return componentBits;
        }

        public List<Component> GetAssociatedComponents(int entityID, EntityBits ComponentBits)
        {
            Matcher myMatcher = new Matcher();
            myMatcher.AddToOrSet(ComponentBits);
            List<Component> myComponents = new List<Component>();
            foreach(var A in _componentMappers)
            {
                if(myMatcher.Matches(A.Key))
                {
                    var Value = A.Value.Get(entityID);
                    myComponents.AddRange((IEnumerable<Component>)Value);
                }
            }
            return myComponents;
        }
    }
}
