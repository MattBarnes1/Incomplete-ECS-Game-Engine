

using System;
using System.Collections.Generic;
using Engine.Entities;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Components;

namespace Engine.Managers
{

    //TODO: Static this class it makes more sense 
    public class ArchetypeManager
    {
        public void DefineArchetype(string ArchetypeName, Action<Component[]> myInitializer, params Type[] types)
        {
            if(myInitializer != null)
            {
                myStringToEntityInitializer.Add(ArchetypeName, myInitializer);
            }
            myStringToEntity.Add(ArchetypeName, types);
        }

        Dictionary<String, Action<Component[]>> myStringToEntityInitializer = new Dictionary<String, Action<Component[]>>();
        Dictionary<String, Type[]> myStringToEntity = new Dictionary<string, Type[]>();

        public Entity CreateEntityFromArchetype(string v, out List<Component> myCreatedComponents)
        {
            Type[] myBits = myStringToEntity[v];
            Entity myEntity = new Entity();
            myCreatedComponents = myEntity.AddComponentsFromTypes(myBits);
            return myEntity;
        }
        public int ArchetypeCount
        {
            get
            {
                return myStringToEntity.Count();
            }
        }

        public int ArchetypeInitializerCount { get { return myStringToEntityInitializer.Count; } }
    }
}
