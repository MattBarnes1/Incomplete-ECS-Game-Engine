using Engine.Compute;
using Engine.ECS_Subsystem.Managers;
using Engine.Renderer;
using Engine.Utilities;
using EngineRenderer;
using EngineRenderer.Interfaces;
using EngineRenderer.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Initializers
{
    /// <summary>
    /// Picks devices for the Vulkan Rendering and VulkanComputer managers, sets up their queues and makes them aware of eachother's ending semaphores so that they properly sync tasks.
    /// </summary>
    public class VulkanInitializer
    {
        public VulkanInitializer(String WindowName, RendererWindow WindowHandle, GraphicsSettings mySettings, IPreferredGraphicsDeviceFilter myGraphicsDeviceFilter = null, IPreferredComputeDeviceFilter myComputeDeviceFilter = null)
        {
            var Window = new VulkanInstance(WindowName, WindowHandle.WindowHandle);
            var Surface = new SurfaceKHR(Window);
            var myDeviceList = new VulkanSupportedDevices(Window, Surface, myGraphicsDeviceFilter, myComputeDeviceFilter);

            var SelectedPhysicalGraphicsDevice = myDeviceList.GetBestGraphicsDevice();
            var SelectedLogicalGraphicsDevice = new VulkanLogicalDevice(SelectedPhysicalGraphicsDevice);

            DescriptorSetPoolManager myDescriptorPoolGraphicsManager;
            DescriptorSetPoolManager myDescriptorPoolComputeManager;
            Renderer.Vulkan.Queue myGraphicsQueue = null;
            Renderer.Vulkan.Queue myComputeQueue = null;

            var SelectedPhysicalComputeDevice = myDeviceList.GetNextComputeDevice();
            VulkanLogicalDevice SelectedLogicalComputeDevice;

            if (myDeviceList.Count == 1)
            {
                //Compute and Renderer will share a device.
                SelectedLogicalComputeDevice = new VulkanLogicalDevice(SelectedPhysicalGraphicsDevice);
                myDescriptorPoolGraphicsManager = new DescriptorSetPoolManager(SelectedLogicalGraphicsDevice);
                myDescriptorPoolComputeManager = myDescriptorPoolGraphicsManager;
                myGraphicsQueue = SelectedLogicalGraphicsDevice.QueueManager.GetQueue(QueueFlags.Graphics);
                myComputeQueue = SelectedLogicalGraphicsDevice.QueueManager.GetQueue(QueueFlags.Compute);
            }
            else
            {
                //TODO: iterate through queue to find next best device for compute
                SelectedLogicalComputeDevice = new VulkanLogicalDevice(SelectedPhysicalComputeDevice);
                myDescriptorPoolGraphicsManager = new DescriptorSetPoolManager(SelectedLogicalGraphicsDevice);
                myDescriptorPoolComputeManager = new DescriptorSetPoolManager(SelectedLogicalComputeDevice);
                myGraphicsQueue = SelectedLogicalGraphicsDevice.QueueManager.GetQueue(QueueFlags.Graphics);
                myComputeQueue = SelectedLogicalComputeDevice.QueueManager.GetQueue(QueueFlags.Compute);
            }


            myLightingManager = new LightingManager();

            var Renderer = new VulkanRenderer(myLightingManager, Surface, Window, SelectedLogicalGraphicsDevice, myDescriptorPoolGraphicsManager, SelectedPhysicalGraphicsDevice, myGraphicsQueue, mySettings);
            var Compute = new VulkanCompute(SelectedLogicalComputeDevice, myDescriptorPoolComputeManager, SelectedPhysicalComputeDevice, myComputeQueue);
            RenderingManager = Renderer;
            //Make sure they know about eachother's fences so they don't draw or do compute shaders at the same time!
            Renderer.SetComputeFence(Compute.GetFinishedFence());//Semaphore or fence?
            Compute.SetRendererFence(Renderer.GetFinshedFence());
            ComputeManager = Compute;

        }
        public RigidBodyManager myPhysicsManager { get; private set; }
        public LightingManager myLightingManager { get; private set; }
        public IRenderingManager RenderingManager { get; private set; }
        public IComputeManager ComputeManager { get; private set; }


    }
}
