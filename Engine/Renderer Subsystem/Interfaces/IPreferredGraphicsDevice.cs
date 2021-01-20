using System.Collections.Generic;
using Vulkan;

namespace EngineRenderer.Interfaces
{
    public interface IPreferredGraphicsDeviceFilter
    {
        int Score(VulkanPhysicalDevice myDevice, SurfaceKHR mySurface);
    }
}