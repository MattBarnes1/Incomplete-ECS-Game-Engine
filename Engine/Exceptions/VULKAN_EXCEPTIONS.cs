using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineRenderer.Exceptions.VULKAN
{
    public class VULKAN_SURFACE_CREATION_FAILED : Exception
    {
        public VULKAN_SURFACE_CREATION_FAILED() : base("Vulkan was unable to create surface for renderer!") { }
    }
    public class VULKAN_NO_HARDWARE_SUPPORTED_DEVICES : Exception
    {
        public VULKAN_NO_HARDWARE_SUPPORTED_DEVICES() : base("No Vulkan Supported GPUs Found!") { }
    }
    public class VULKAN_NO_APPLICATION_SUPPORTED_DEVICES : Exception
    {
        public VULKAN_NO_APPLICATION_SUPPORTED_DEVICES() : base("Application Couldn't find appropriate GPU!") { }
    }
}
