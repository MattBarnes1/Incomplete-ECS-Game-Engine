using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineECS.Systems.System_Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed public class SystemReaderIgnores : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type[] myComponentTypes;
        // This is a positional argument
        public SystemReaderIgnores(params Type[] myComponentType)
        {
            this.myComponentTypes = myComponentType;
        }

        public Type[] ComponentType
        {
            get { return myComponentTypes; }
        }
    }
}
