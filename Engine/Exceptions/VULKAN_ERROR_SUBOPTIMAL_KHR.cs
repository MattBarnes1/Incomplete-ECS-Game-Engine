using System;
using System.Runtime.Serialization;

namespace EngineRenderer.Exceptions.VULKAN
{
    [Serializable]
    internal class VULKAN_ERROR_SUBOPTIMAL_KHR : Exception
    {
        public VULKAN_ERROR_SUBOPTIMAL_KHR()
        {
        }

        public VULKAN_ERROR_SUBOPTIMAL_KHR(string message) : base(message)
        {
        }

        public VULKAN_ERROR_SUBOPTIMAL_KHR(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VULKAN_ERROR_SUBOPTIMAL_KHR(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}