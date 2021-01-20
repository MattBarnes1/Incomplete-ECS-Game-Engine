using Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed partial class SystemReader : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type[] myComponentTypes;
        // This is a positional argument
        public SystemReader(params Type[] myComponentType)
        {
            this.myComponentTypes = myComponentType;
        }

#if DEBUG

        public SystemReader(int NumberCount, params Type[] MustHaveComponents)
        {
            List<Type> myComponentsToWRite;
            if (MustHaveComponents != null)
                myComponentsToWRite = new List<Type>(MustHaveComponents);
            else
                myComponentsToWRite = new List<Type>();

            List<Type> Found = typeof(Component).Assembly.GetTypes().Where<Type>(delegate (Type T)
            {
                return (T.IsSubclassOf(typeof(Component)));
            }).ToList();

            Random myRandom = new Random();

            for(int i = 0; i < Found.Count; i++)
            {
                int Selected = myRandom.Next() % Found.Count;

                int NewPosition = myRandom.Next() % myComponentsToWRite.Count;

                myComponentsToWRite.Insert(NewPosition, Found[Selected]);

            }
            myComponentTypes = myComponentsToWRite.ToArray();
        }






#endif

        public Type[] ComponentType
        {
            get { return myComponentTypes; }
        }
        
    }
}
