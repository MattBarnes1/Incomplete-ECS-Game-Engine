using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS.Systems.System_Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false)]
    sealed class LinkToSystemAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236


        // This is a positional argument
        public LinkToSystemAttribute(params Type[] myTypes)
        {
            this.Types = myTypes;
            foreach(Type A in Types)
            {
                if(!A.IsSubclassOf(typeof(Engine.Systems.System)))
                {
                    throw new Exception("Invalid System Linking Detected.");
                }
            }
        }

        public Type[] Types { get; }
    }
}
