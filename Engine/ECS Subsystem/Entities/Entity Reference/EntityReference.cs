

using Engine.Components;
using System;
using System.Collections.Generic;

namespace Engine.Entities
{
    public abstract class EntityReference : IComparer<EntityReference>, IComparable<EntityReference>
    {
        public Entity myEntity { get; private set; }
        public uint ID {
            get
            {
                return (uint)myEntity.ID;
            }
        }
        public EntityReference() { }

        public virtual void CopyFrom(EntityReference t)
        {
            myEntity = t.myEntity;
        }

        public virtual void CopyFrom(Entity MyEntity)
        {
            myEntity = MyEntity;
        }

        public abstract IReadOnlyList<T> Get<T>() where T : Component;

        public int Compare(EntityReference x, EntityReference y)
        {
            return x.ID.CompareTo(y.ID);
        }

        public int CompareTo(EntityReference other)
        {
            return ID.CompareTo(other.ID);
        }

        public override int GetHashCode()
        {
            return myEntity.ID;
        }
    }

    public class EntityReference<ComponentType1> : EntityReference where ComponentType1 : Component, new()
    {
        public IReadOnlyList<ComponentType1> Component1;
        public EntityReference()
        {
        }
        public override void CopyFrom(EntityReference t)
        {
            base.CopyFrom(t);
            var Test = t as EntityReference<ComponentType1>;
            Component1 = Test.Component1;
        }

        public override void CopyFrom(Entity MyEntity)
        {
            base.CopyFrom(MyEntity);
            Component1 = (IReadOnlyList<ComponentType1>)MyEntity.myComponentManager.GetComponentsByTypeReadonly<ComponentType1>(MyEntity);

        }

        public override IReadOnlyList<T> Get<T>()
        {
            if (typeof(ComponentType1) == typeof(T))
            {
                return (IReadOnlyList<T>)Component1;
            }
            return null;
        }
    }
    public class EntityReference<ComponentType1, ComponentType2> : EntityReference<ComponentType1> 
        where ComponentType1 : Component, new() 
        where ComponentType2 : Component, new()
    {
        public IReadOnlyList<ComponentType2> Component2;
        public EntityReference() 
        {
        }
        public override void CopyFrom(EntityReference t)
        {
            base.CopyFrom(t);
            var Test = t as EntityReference<ComponentType1, ComponentType2>;
            Component2 = Test.Component2;

        }
        public override void CopyFrom(Entity MyEntity)
        {
            base.CopyFrom(MyEntity);
            Component2 = (IReadOnlyList<ComponentType2>)MyEntity.myComponentManager.GetComponentsByTypeReadonly<ComponentType2>(MyEntity);

        }
        public override IReadOnlyList<T> Get<T>()
        {
            var Return = base.Get<T>();
            if (Return == null)
            {
                if (typeof(ComponentType2) == typeof(T))
                {
                    return (IReadOnlyList<T>)Component2;
                }
                return null;
            }
            else
            {
                return Return;
            }
        }
    }
    public class EntityReference<ComponentType1, ComponentType2, ComponentType3> : EntityReference<ComponentType1, ComponentType2>
        where ComponentType1 : Component, new()
        where ComponentType2 : Component, new()
        where ComponentType3 : Component, new()
    {
        public IReadOnlyList<ComponentType3> Component3;
        public EntityReference()
        {
        }
        public override void CopyFrom(EntityReference t)
        {
            base.CopyFrom(t);
            var Test = t as EntityReference<ComponentType1, ComponentType2, ComponentType3>;
            Component3 = Test.Component3;

        }
        public override void CopyFrom(Entity MyEntity)
        {
            base.CopyFrom(MyEntity);
            Component3 = MyEntity.myComponentManager.GetComponentsByTypeReadonly<ComponentType3>(MyEntity);

        }
        public override IReadOnlyList<T> Get<T>()
        {
            var Return = base.Get<T>();
            if (Return == null)
            {
                if (typeof(ComponentType3) == typeof(T))
                {
                    return (IReadOnlyList<T>)Component3;
                }
                return null;
            }
            else
            {
                return Return;
            }
        }

    }
    public class EntityReference<ComponentType1, ComponentType2, ComponentType3, ComponentType4> : EntityReference<ComponentType1, ComponentType2, ComponentType3>
        where ComponentType1 : Component, new()
        where ComponentType2 : Component, new()
        where ComponentType3 : Component, new()
        where ComponentType4 : Component, new()
    {
        public IReadOnlyList<ComponentType4> Component4;
        public EntityReference()
        {
        }
        public override void CopyFrom(EntityReference t)
        {
            base.CopyFrom(t);
            var Test = t as EntityReference<ComponentType1, ComponentType2, ComponentType3, ComponentType4>;
            Component4 = Test.Component4;

        }
        public override void CopyFrom(Entity MyEntity)
        {
            base.CopyFrom(MyEntity);
            Component4 = MyEntity.myComponentManager.GetComponentsByTypeReadonly<ComponentType4>(MyEntity);

        }
        public override IReadOnlyList<T> Get<T>()
        {
            var Return = base.Get<T>();
            if (Return == null)
            {
                if (typeof(ComponentType4) == typeof(T))
                {
                    return (IReadOnlyList<T>)Component4;
                }
                return null;
            }
            else
            {
                return Return;
            }
        }
    }
    public class EntityReference<ComponentType1, ComponentType2, ComponentType3, ComponentType4, ComponentType5> : EntityReference<ComponentType1, ComponentType2, ComponentType3, ComponentType4> 
        where ComponentType1 : Component, new()
        where ComponentType2 : Component, new()
        where ComponentType3 : Component, new()
        where ComponentType4 : Component, new()
        where ComponentType5 : Component, new()
    {
        public IReadOnlyList<ComponentType5> Component5;
        public EntityReference()
        {

        }
        public override void CopyFrom(EntityReference t)
        {
            base.CopyFrom(t);
            var Test = t as EntityReference<ComponentType1, ComponentType2, ComponentType3, ComponentType4, ComponentType5>;
            Component5 = Test.Component5;

        }
        public override void CopyFrom(Entity MyEntity)
        {
            base.CopyFrom(MyEntity);
            Component5 = MyEntity.myComponentManager.GetComponentsByTypeReadonly<ComponentType5>(MyEntity);
        }
        public override IReadOnlyList<T> Get<T>()
        {
            var Return = base.Get<T>();
            if (Return == null)
            {
                if (typeof(ComponentType5) == typeof(T))
                {
                    return (IReadOnlyList<T>)Component5;
                }
                return null;
            }
            else
            {
                return Return;
            }
        }
    }
    public class EntityReference<ComponentType1, ComponentType2, ComponentType3, ComponentType4, ComponentType5, ComponentType6> : EntityReference<ComponentType1, ComponentType2, ComponentType3, ComponentType4, ComponentType5>
        where ComponentType1 : Component, new()
        where ComponentType2 : Component, new()
        where ComponentType3 : Component, new()
        where ComponentType4 : Component, new()
        where ComponentType5 : Component, new()
        where ComponentType6 : Component, new()
    {
        public IReadOnlyList<ComponentType6> Component6;
        public EntityReference()
        {

        }
        public override void CopyFrom(EntityReference t)
        {
            base.CopyFrom(t);
            var Test = t as EntityReference<ComponentType1, ComponentType2, ComponentType3, ComponentType4, ComponentType5, ComponentType6>;
            Component6 = Test.Component6;
        }

        public override void CopyFrom(Entity MyEntity)
        {
            base.CopyFrom(MyEntity);
            Component6 = MyEntity.myComponentManager.GetComponentsByTypeReadonly<ComponentType6>(MyEntity);
        }
        public override IReadOnlyList<T> Get<T>()
        {
            var Return = base.Get<T>();
            if(Return == null)
            {
                if (typeof(ComponentType6) == typeof(T))
                {
                    return (IReadOnlyList<T>)Component6;
                }
                return null;
            }
            else
            {
                return Return;
            }
        }
    }
}
