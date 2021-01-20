using System;

namespace Engine.Systems
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed public class SystemWriterIgnores : Attribute //TODO: Actually implement this in the class.
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type[] myComponentTypes;
        // This is a positional argument
        public SystemWriterIgnores(params Type[] myComponentType)
        {
            this.myComponentTypes = myComponentType;
        }

        public Type[] ComponentType
        {
            get { return myComponentTypes; }
        }
    }
}
