using System;
using System.Runtime.Serialization;

namespace Engine.Exceptions
{
    [Serializable]
    internal class INVALID_COMPONENT_COPY_ATTEMPT : Exception
    {
        private Type type;

        public INVALID_COMPONENT_COPY_ATTEMPT()
        {
        }

        public INVALID_COMPONENT_COPY_ATTEMPT(Type type)
        {
            this.type = type;
        }

        public INVALID_COMPONENT_COPY_ATTEMPT(string message) : base(message)
        {
        }

        public INVALID_COMPONENT_COPY_ATTEMPT(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected INVALID_COMPONENT_COPY_ATTEMPT(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}