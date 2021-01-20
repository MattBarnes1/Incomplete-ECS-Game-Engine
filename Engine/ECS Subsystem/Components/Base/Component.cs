
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Components
{
    
    public abstract class Component
    {
        public Component()
        {
        }

        public virtual void CopyFrom<T>(T Component) where T:Component
        {
            throw new Exception("Component CopyFrom was used but not created in Component!");
        }

    }
}
