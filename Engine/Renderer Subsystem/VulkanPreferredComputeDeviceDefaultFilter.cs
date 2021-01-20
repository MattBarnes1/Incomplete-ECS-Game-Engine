using EngineRenderer;
using EngineRenderer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Renderer
{
    public class VulkanPreferredComputeDeviceDefaultFilter : IPreferredComputeDeviceFilter
    {
        public int Score(VulkanPhysicalDevice myDevice)
        {
            return 100;
        }
    }
}
