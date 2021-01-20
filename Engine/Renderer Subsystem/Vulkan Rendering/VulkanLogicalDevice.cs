using Engine.Renderer.Vulkan;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop = Vulkan;
namespace EngineRenderer.Vulkan
{
    public class VulkanLogicalDevice : Interop.Device
    {
        public VulkanPhysicalDevice myAssociatedDevice { get; internal set; }

        public DeviceQueueManager QueueManager { get; private set; }


        public VulkanLogicalDevice(VulkanPhysicalDevice selectedPhysicalDevice)
        {
            myAssociatedDevice = selectedPhysicalDevice;
            selectedPhysicalDevice.LogicalDevice = this;
            QueueManager = new DeviceQueueManager(selectedPhysicalDevice, this);

            var DeviceCreateInfo = new Interop.DeviceCreateInfo
            {
                EnabledExtensionNames = new string[] { "VK_KHR_swapchain", },
                QueueCreateInfos = QueueManager.GetDeviceQueueCreateArray()// new Interop.DeviceQueueCreateInfo[] { GraphicsQueueInfo, ComputeQueueInfo }
            };
            unsafe
			{
				Interop.Result result;
				fixed (IntPtr* ptrpDevice = &this.m)
				{
					result = Interop.Interop.NativeMethods.vkCreateDevice(selectedPhysicalDevice.m, DeviceCreateInfo.m, null, ptrpDevice);
				}
				if (result != Interop.Result.Success)
					throw new Interop.ResultException(result);
            }
            DeviceCreateInfo.Dispose();

        }
    }
}
