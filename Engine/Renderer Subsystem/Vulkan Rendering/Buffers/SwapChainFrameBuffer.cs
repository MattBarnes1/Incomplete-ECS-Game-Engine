using Engine.Renderer.Vulkan;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer.Vulkan_Rendering.Managers.SwapChain_Management
{
    public class SwapChainFrameBuffer : FrameBuffer
    {
        public SwapChainFrameBuffer(uint width, int BufferCount, Image aImage, uint height, uint layer = 1) : base(width, height, layer)
        {
            CreateAttachment("Swapchain Buffer " + BufferCount, VulkanRenderer.Surface.SelectedSurfaceFormat.Format, ImageViewType.View2D, aImage, ImageAspectFlags.Color, 1, 1);
        }

        public SwapChainFrameBuffer(RenderPass myRenderPass, Image aImage, int BufferCount, uint width, uint height, uint layer = 1) : base(myRenderPass, width, height, layer)
        {
            CreateAttachment("Swapchain Buffer " + BufferCount, VulkanRenderer.Surface.SelectedSurfaceFormat.Format, ImageViewType.View2D, aImage, ImageAspectFlags.Color, 1, 1);
        }
    }
}
