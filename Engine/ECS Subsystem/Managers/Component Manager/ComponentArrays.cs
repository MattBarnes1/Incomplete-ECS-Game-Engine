using Engine.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Managers
{
    public class ComponentArray //TODO: Phase these out.
    {
        List<EntityBits> myArrays = new List<EntityBits>();
        public void Add(EntityBits EntityBits)
        {
            myArrays.Add(EntityBits);
        }

        public EntityBits Get()
        {
            EntityBits Result = null;
            if (myArrays.Count != 0)
            {
                Result = new EntityBits();
                foreach (EntityBits A in myArrays)
                {
                    Result = Result | (A);
                }
            }

            return Result;
        }

    }
}
