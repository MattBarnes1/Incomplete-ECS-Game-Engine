using System;
using System.Runtime.Serialization;
namespace Engine.Exceptions
{
    [Serializable]
    public class InvalidEntityException : Exception
    {
        public InvalidEntityException() { }
        public InvalidEntityException(string message) : base("Component was added to Destroyed Entity!") { }
        public InvalidEntityException(string message, Exception inner) : base(message, inner) { }
        protected InvalidEntityException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}