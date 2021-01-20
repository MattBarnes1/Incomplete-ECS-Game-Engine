using Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS.Components.Base
{


    public abstract class SingletonComponent<T> : Component where T:class
    {
        static T myComponent;
        protected SingletonComponent()
        {
            if (myComponent != null) throw new Exception("Component Singleton redeclared instead of reusing!");
            myComponent = this as T;
            if (myComponent == null) throw new Exception("Component Singleton Failed to Construct!");

        }

        public static T Get()
        {
            if(myComponent == null)
            {
                typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            return myComponent;
        }


    }
}
