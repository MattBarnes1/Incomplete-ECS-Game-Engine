using Engine.Renderer.Vulkan;
using Engine.Renderer.Vulkan.Descriptor_Sets;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer.Vulkan_Rendering.Buffers
{
    public class GeometryBuffer : FrameBuffer
    {
        public GeometryBuffer() : base((uint)VulkanRenderer.Viewport.Width, (uint)VulkanRenderer.Viewport.Height, 1)
        //VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT
        {//VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT
            base.CreateAttachment("Position", Format.R16G16B16A16Sfloat, ImageType.Image2D, ImageViewType.View2D, ImageUsageFlags.ColorAttachment | ImageUsageFlags.Sampled, ImageAspectFlags.Color, ImageTiling.Optimal, SampleCountFlags.Count1, 1, 1); //Worldspace positions
            base.CreateAttachment("Normal", Format.R16G16B16A16Sfloat, ImageType.Image2D, ImageViewType.View2D, ImageUsageFlags.ColorAttachment | ImageUsageFlags.Sampled, ImageAspectFlags.Color, ImageTiling.Optimal, SampleCountFlags.Count1, 1, 1); //Worldspace positions
            base.CreateAttachment("Diffuse", Format.R8G8B8A8Unorm, ImageType.Image2D, ImageViewType.View2D, ImageUsageFlags.ColorAttachment | ImageUsageFlags.Sampled, ImageAspectFlags.Color, ImageTiling.Optimal, SampleCountFlags.Count1, 1, 1); //Worldspace positions
            base.CreateAttachment("Depth Buffer", VulkanRenderer.SelectedPhysicalDevice.DepthBufferFormat, ImageType.Image2D, ImageViewType.View2D, ImageUsageFlags.DepthStencilAttachment | ImageUsageFlags.Sampled, ImageAspectFlags.Depth | ImageAspectFlags.Stencil, ImageTiling.Optimal, SampleCountFlags.Count1, 1, 1); //TODO: double check myDevice.SelectedSurfaceFormat.Format

        }

    }
}
