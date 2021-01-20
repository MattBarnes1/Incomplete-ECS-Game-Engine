using Engine.Renderer;
using Engine.Renderer.Vulkan;
using EngineRenderer;
using EngineRenderer.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Compute
{
    public class VulkanCompute : IComputeManager
    {
        private VulkanLogicalDevice selectedLogicalGraphicsDevice;
        private DescriptorSetPoolManager myDescriptorPoolComputeManager;
        private VulkanPhysicalDevice selectedPhysicalGraphicsDevice;
        private Engine.Renderer.Vulkan.Queue myComputeQueue;

        public VulkanCompute(VulkanLogicalDevice selectedLogicalGraphicsDevice, DescriptorSetPoolManager myDescriptorPoolComputeManager, VulkanPhysicalDevice selectedPhysicalGraphicsDevice, Engine.Renderer.Vulkan.Queue myComputeQueue)
        {
            this.selectedLogicalGraphicsDevice = selectedLogicalGraphicsDevice;
            this.myDescriptorPoolComputeManager = myDescriptorPoolComputeManager;
            this.selectedPhysicalGraphicsDevice = selectedPhysicalGraphicsDevice;
            this.myComputeQueue = myComputeQueue;
            FenceCreateInfo fenceInfo = new FenceCreateInfo();
            myComputeFence = selectedLogicalGraphicsDevice.CreateFence(fenceInfo);
        }

        Fence myComputeFence;
        Fence myDrawingFence;

        internal void SetRendererFence(Fence p)
        {
            myDrawingFence = p;
        }

        public Fence GetFinishedFence()
        {
            return myComputeFence;
        }
    }
}
