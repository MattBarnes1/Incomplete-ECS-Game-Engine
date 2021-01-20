
using Engine.Components;
using Engine.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Engine.Managers
{
    public static class ComponentRegistration
    {

        public static void Initialize()
        {
           var Assembly2 = Assembly.GetEntryAssembly();
           var Results = Assembly2.GetTypes().Where<Type>(delegate (Type T)
            {
                if(T.IsSubclassOf(typeof(Component)))
                {
                    Register(T);
                    return true;
                }
                return false;
            });
        }

        private static Dictionary<Type, EntityBits> _componentTypes = new Dictionary<Type, EntityBits>();
        private static Dictionary<EntityBits, Type> _componentBitvectorToTypes = new Dictionary<EntityBits, Type>();
        public static void Register(Type myType)
        {
            if (_componentTypes.TryGetValue(myType, out var id))
                return;

            EntityBits myVector = new EntityBits();
            myVector[_componentTypes.Count()] = true;
            Debug.WriteLine("Registered Component: " + myType + " : " + myVector);
            _componentTypes.Add(myType, myVector);
            _componentBitvectorToTypes.Add(myVector, myType);


        }

        public static void Clear()
        {
            _componentTypes.Clear();
            _componentBitvectorToTypes.Clear();
        }

        public static EntityBits GetComponentBitset(Component myComponent1)
        {
            return GetComponentBitset(myComponent1.GetType());
        }

        public static EntityBits GetComponentBitset(Type type)
        {
            if (_componentTypes.TryGetValue(type, out var id))
                return id;
            if (!type.IsSubclassOf(typeof(Component)))
                throw new Exception("Invalid Component Type Added: " + type);


            Register(type);
            return _componentTypes[type];
        }




    }
}
