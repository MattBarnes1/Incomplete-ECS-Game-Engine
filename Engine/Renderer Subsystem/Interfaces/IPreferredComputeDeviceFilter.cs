using System.Collections.Generic;
using Vulkan;

namespace EngineRenderer.Interfaces
{
    public interface IPreferredComputeDeviceFilter
    {
        int Score(VulkanPhysicalDevice myDevice);
    }
}