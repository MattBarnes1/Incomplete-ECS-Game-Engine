using EngineRenderer;
using EngineRenderer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer
{
    class VulkanPreferredGraphicsDeviceDefaultFilter : IPreferredGraphicsDeviceFilter
    {
        public int Score(VulkanPhysicalDevice myDevice, SurfaceKHR mySurface)
        {
            mySurface.SurfaceCapabilities = myDevice.GetSurfaceCapabilitiesKHR(mySurface);
            var result = GetDefaultSurfaceFormat(myDevice.GetSurfaceFormatsKHR(mySurface));
            mySurface.SelectedSurfaceFormat = result.GetValueOrDefault();
            int returnVal = -1;
            if (result.HasValue)
            {
                for (int i = 0; i < myDevice.QueueFamilyProperties.Length; i++)
                {
                    if ((myDevice.QueueFamilyProperties[i].QueueFlags & QueueFlags.Graphics) == QueueFlags.Graphics)
                    {
                        if (myDevice.GetSurfaceSupportKHR((uint)i, mySurface))
                        {
                            returnVal = 100;
                            break;
                        }
                    }
                }
            }

            return returnVal;
        }

        private SurfaceFormatKHR? GetDefaultSurfaceFormat(SurfaceFormatKHR[] supportedSurfaceFormats)
        {
            var FormatReturn = Format.Undefined;
            foreach (var f in supportedSurfaceFormats)
            {
                if (f.Format == Format.B8G8R8A8Unorm || f.Format == Format.R8G8B8A8Unorm)
                    return f;
            }
            return null;
        }
    }
}
