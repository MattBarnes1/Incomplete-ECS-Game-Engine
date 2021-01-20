using System;
using System.Runtime.Serialization;

namespace EngineRenderer
{
    [Serializable]
    internal class VULKAN_NO_APPLICATION_SUPPORTED_SURFACES : Exception
    {
        public VULKAN_NO_APPLICATION_SUPPORTED_SURFACES()
        {
        }

        public VULKAN_NO_APPLICATION_SUPPORTED_SURFACES(string message) : base(message)
        {
        }

        public VULKAN_NO_APPLICATION_SUPPORTED_SURFACES(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VULKAN_NO_APPLICATION_SUPPORTED_SURFACES(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}