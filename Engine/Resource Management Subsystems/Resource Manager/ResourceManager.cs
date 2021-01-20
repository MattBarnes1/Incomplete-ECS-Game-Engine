using Engine.Components;
using Engine.Resource_Manager.LRUCache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Resources
{
    public class ResourceManager
    {
        static ResourceManager myManager;
        static LRUCache myCache = new LRUCache(20);
        static public string ContentDirectory { get; private set; }


        public int GetReaderCount<T>() where T : class
        {
            return ContentReaderByType<T>.Count;
        }


        public ResourceManager(string v = "./Content")
        {
            Type[] Test = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var A in Test)
            {
                if((A.IsSubclassOf(typeof(ContentReader)) && A != typeof(ContentReader<>)  && A != typeof(ContentReader)))
                {
                    A.GetConstructor(new Type[] { }).Invoke(new object[] { });
                    Debug.WriteLine(A + " reader was added!");
                }
            }

            ContentDirectory = Path.GetFullPath(v);
#if DEBUG
            Debug.WriteLine("ContentDirectory: " + ContentDirectory);
#endif
            myManager = this;
        }
        public static T Load<T>(string v) where T : class
        {
            T Result = myCache.Get<T>(v);
            if(Result == null)
            {
                Result = ContentReaderByType<T>.Load(Path.Combine(ContentDirectory, v));
                myCache.Insert(Result, v);
            }
            return Result;        
        }



        public static void LoadInto<T>(T aComponent, string FileName) where T : Components.Component
        {
            T Result = myCache.Get<T>(FileName);
            if (Result == null)
            {
                Result = ContentReaderByType<T>.Load(Path.Combine(ContentDirectory, FileName));
                myCache.Insert(Result, FileName);
            }
            aComponent.CopyFrom<T>(Result);
        }
    }
}
