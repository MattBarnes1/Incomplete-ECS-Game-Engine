using Engine.Renderer;
using Engine.Renderer.Vulkan;
using Engine.Renderer.Vulkan_Rendering.Managers.SwapChain_Management;
using EngineRenderer;
using EngineRenderer.Exceptions.VULKAN;
using EngineRenderer.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Low_Level.Vulkan.Classes_By_Me.Frame_Buffer_Pool
{
    public class SwapchainManager : IDisposable
    {
        private void CreateDefaultSwapchainRenderPass(Device SelectedLogicalGraphicsDevice)
        {
            var anAttachmentDescription = new AttachmentDescription
            {
                Format = VulkanRenderer.Surface.SelectedSurfaceFormat.Format,
                Samples = SampleCountFlags.Count1,
                LoadOp = AttachmentLoadOp.Clear,
                StoreOp = AttachmentStoreOp.Store,
                StencilLoadOp = AttachmentLoadOp.DontCare,
                StencilStoreOp = AttachmentStoreOp.DontCare,
                InitialLayout = ImageLayout.Undefined,
                FinalLayout = ImageLayout.PresentSrcKHR
            };
            var anAttachmentReference = new AttachmentReference { Layout = ImageLayout.ColorAttachmentOptimal };

         

            var aSubpassDescription = new SubpassDescription
            {
                PipelineBindPoint = PipelineBindPoint.Graphics,
                ColorAttachments = new AttachmentReference[] { anAttachmentReference }
            };

            var aRenderPassCreateInfo = new RenderPassCreateInfo
            {
                Attachments = new AttachmentDescription[] { anAttachmentDescription },
                Subpasses = new SubpassDescription[] { aSubpassDescription }
            };
            myBaseRenderPass = SelectedLogicalGraphicsDevice.CreateRenderPass(aRenderPassCreateInfo);
        }
        RenderPassBeginInfo renderPassBeginInfo;
        RenderPass myBaseRenderPass;
        public SwapchainManager(Device LogicalDevice, SurfaceFormatKHR SelectedSurfaceFormat)
        {

            var compositeAlpha = VulkanRenderer.Surface.SurfaceCapabilities.SupportedCompositeAlpha.HasFlag(CompositeAlphaFlagsKHR.Inherit)
               ? CompositeAlphaFlagsKHR.Inherit
               : CompositeAlphaFlagsKHR.Opaque;
            SemaphoreCreateInfo semaphoreInfo = new SemaphoreCreateInfo();
            SwapchainDrawCompleteSemaphore = LogicalDevice.CreateSemaphore(semaphoreInfo);
            CreateDefaultSwapchainRenderPass(LogicalDevice);
            renderPassBeginInfo = new RenderPassBeginInfo { ClearValueCount = 1, ClearValues = new ClearValue[] { new ClearValue { Color = new ClearColorValue(Color.AliceBlue) } }, RenderPass = myBaseRenderPass };
            renderPassBeginInfo.RenderArea = new Rect2D { Extent = new Extent2D { Width = VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent.Width, Height = VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent.Height }, Offset = new Offset2D { X = 0, Y = 0 } };
            var swapchainInfo = new SwapchainCreateInfoKHR
            {
                Surface = VulkanRenderer.Surface,
                MinImageCount = VulkanRenderer.Surface.SurfaceCapabilities.MinImageCount + 1,
                ImageFormat = VulkanRenderer.Surface.SelectedSurfaceFormat.Format,
                ImageColorSpace = VulkanRenderer.Surface.SelectedSurfaceFormat.ColorSpace,
                ImageExtent = VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent,
                ImageUsage = ImageUsageFlags.ColorAttachment,
                PreTransform = SurfaceTransformFlagsKHR.Identity,
                ImageArrayLayers = 1,
                ImageSharingMode = SharingMode.Exclusive,
                QueueFamilyIndices = new uint[] { VulkanRenderer.ActiveGraphicsFamilyQueueIndex },
                PresentMode = PresentModeKHR.Fifo,
                CompositeAlpha = compositeAlpha
            };
            Swapchain = VulkanRenderer.SelectedLogicalDevice.CreateSwapchainKHR(swapchainInfo);
            Images = VulkanRenderer.SelectedLogicalDevice.GetSwapchainImagesKHR(Swapchain);
            SwapChainPresentInfo = new PresentInfoKHR
            {
                Swapchains = new SwapchainKHR[] { Swapchain },
                ImageIndices = new uint[] { VulkanRenderer.SelectedLogicalDevice.AcquireNextImageKHR(Swapchain, ulong.MaxValue, SwapchainDrawCompleteSemaphore) }
            };


            Framebuffers = new SwapChainFrameBuffer[Images.Length]; //Move this to swap chains

            for (int i = 0; i < Images.Length; i++)
            {

                Framebuffers[i] = new SwapChainFrameBuffer(myBaseRenderPass, Images[i], i, VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent.Width, VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent.Height, 1);
                Framebuffers[i].Initialize();
            }
        }


        public PresentInfoKHR SwapChainPresentInfo { get; }

        public void SwapBuffer()
        {
            GetNextSwapIndex();
        }




        private void GetNextSwapIndex()
        {
            SwapChainPresentInfo.ImageIndices[0] = VulkanRenderer.SelectedLogicalDevice.AcquireNextImageKHR(Swapchain, ulong.MaxValue, SwapchainDrawCompleteSemaphore);
        }

        public void Dispose()
        {
            if (Swapchain != null)
                VulkanRenderer.SelectedLogicalDevice.DestroySwapchainKHR(Swapchain);
            if (SwapchainDrawCompleteSemaphore != null)
                VulkanRenderer.SelectedLogicalDevice.DestroySemaphore(SwapchainDrawCompleteSemaphore);
            for (int i = 0; i < Images.Length; i++)
            {
                Framebuffers[i].Dispose();
                Images[i].Dispose();
            }
        }

        internal RenderPassBeginInfo GetRenderPassBegin()
        {
            renderPassBeginInfo.Framebuffer = GetNextFramebuffer();
            return renderPassBeginInfo;
        }

        public uint Count { get { return (uint)Images.Length; } }
        SwapchainKHR Swapchain;
        Image[] Images { get; set; }

        private SwapChainFrameBuffer[] Framebuffers;

        public Semaphore SwapchainDrawCompleteSemaphore { get; private set; }

        public FrameBuffer GetNextFramebuffer()
        {
           return Framebuffers[SwapChainPresentInfo.ImageIndices[0]];
        }
    }
}
