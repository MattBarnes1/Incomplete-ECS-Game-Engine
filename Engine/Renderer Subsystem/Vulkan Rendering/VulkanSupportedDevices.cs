
using EngineRenderer.Interfaces;
using EngineRenderer.Exceptions.VULKAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
using Engine.Renderer;

namespace EngineRenderer
{
    /// <summary>
    /// This class is used to quickly create a list to query which devices can be used to compute or be used in the Renderer.
    /// </summary>
    public class VulkanSupportedDevices
    {
        public int Count { get { return myDevices.Count(); } }
        class Support
        {
           public VulkanPhysicalDevice myDevice;
           public int GraphicsDeviceScore;
            public int ComputeDeviceScore;
            public Support(VulkanPhysicalDevice myDevice, int GraphicsDeviceScore, int ComputeDeviceScore)
            {
                this.myDevice = myDevice;
                this.GraphicsDeviceScore = GraphicsDeviceScore;
                this.ComputeDeviceScore = ComputeDeviceScore;
            }
        }
        VulkanPhysicalDevice myDevice;
        List<Support> myDevices = new List<Support>();
        IPreferredGraphicsDeviceFilter GraphicsDevicePreferences = new VulkanPreferredGraphicsDeviceDefaultFilter();
        IPreferredComputeDeviceFilter ComputeDevicePreferences = new VulkanPreferredComputeDeviceDefaultFilter();

        public VulkanSupportedDevices(VulkanInstance myWindow, SurfaceKHR mySurface, IPreferredGraphicsDeviceFilter CustomGraphicFilter = null, IPreferredComputeDeviceFilter CustomComputeFilter = null)
        {
            if(CustomGraphicFilter != null)
            {
                GraphicsDevicePreferences = CustomGraphicFilter;
            }
            if (CustomComputeFilter != null)
            {
                ComputeDevicePreferences = CustomComputeFilter;
            }
            foreach (var A in myWindow.EnumeratePhysicalDevices())
            {
                VulkanPhysicalDevice myPhysicalDevice = (VulkanPhysicalDevice)A;
                myDevices.Add(new Support(A, GraphicsDevicePreferences.Score(A, mySurface), ComputeDevicePreferences.Score(A)));                
            }
        }

        public VulkanPhysicalDevice GetBestGraphicsDevice()
        {
            myDevices.Sort((a, b) => a.GraphicsDeviceScore.CompareTo(b.GraphicsDeviceScore));
            if (myDevices.Count() == 0 || myDevices.Last().GraphicsDeviceScore == -1)
            {
                throw new VULKAN_NO_APPLICATION_SUPPORTED_DEVICES();
            }
            return myDevices.Last().myDevice;
        }

        public VulkanPhysicalDevice GetNextComputeDevice()
        {
            myDevices.Sort((a, b) => a.ComputeDeviceScore.CompareTo(b.ComputeDeviceScore));
            if (myDevices.Count() == 0 || myDevices.Last().ComputeDeviceScore == -1)
            {
                throw new VULKAN_NO_APPLICATION_SUPPORTED_DEVICES();
            }
            return myDevices.Last().myDevice;
        }

    }
}
