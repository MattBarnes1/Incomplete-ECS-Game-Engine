using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
using Interoperator = Vulkan.Interop;

namespace Engine.Renderer.Vulkan
{
    public class FrameBuffer :  Framebuffer, IDisposable //Extends class so that underlying code, when changed by the Vulkan Sharp auto generator, won't override this!
    {
		FramebufferCreateInfo myFramebufferInfo = new FramebufferCreateInfo();
		int Width, Height;
		Dictionary<String, FrameBufferAttachment> myAttachments;
		bool Initialized;

		public ReadOnlyCollection<FrameBufferAttachment> GetAttachments()
		{
			return myAttachments.Values.ToList().AsReadOnly();
		}

		public FrameBufferAttachment this[String incomingString]
		{
			get
			{
				return myAttachments[incomingString];
			}
		}
		public FrameBuffer(uint width, uint height, uint layer = 1)
		{
			myAttachments = new Dictionary<string, FrameBufferAttachment>();
			Initialized = false;
			myFramebufferInfo.Width = width;
			myFramebufferInfo.RenderPass = null;
			myFramebufferInfo.Height = height;
			myFramebufferInfo.Layers = layer;
		}

		public FrameBuffer(EngineRenderer.RenderPass myRenderPass, uint width, uint height, uint layer = 1)
		{
			myAttachments = new Dictionary<string, FrameBufferAttachment>();
			Initialized = false;
			myFramebufferInfo.Width = width;
			myFramebufferInfo.RenderPass = myRenderPass;
			myFramebufferInfo.Height = height;
			myFramebufferInfo.Layers = layer;
			Initialize();
		}

		public FrameBufferAttachment CreateAttachment(string AttachmentLookupID, Format myFormat, ImageType myType, ImageViewType view2D, ImageUsageFlags myImageFlag, ImageAspectFlags color, ImageTiling myTiles, SampleCountFlags mySampleCount, uint levelCount, uint layercount)
		{//TODO: Write exception class
			if (Initialized) throw new Exception("Attempted to add attachment to initialized Framebuffer!");
			if (myAttachments.ContainsKey(AttachmentLookupID)) throw new Exception("A framebuffer attachment with ID: " + AttachmentLookupID + " was added to frame buffer twice.");
			ImageCreateInfo myInfo = new ImageCreateInfo() { ImageType = myType, MipLevels = 1, Format = myFormat, Extent = new Extent3D() { Width = myFramebufferInfo.Width, Height = myFramebufferInfo.Height, Depth = 1 }, ArrayLayers = layercount, Samples = mySampleCount, Tiling = myTiles, Usage = myImageFlag };
			myImage = new Image(myInfo);
			var Return = this.CreateAttachment(AttachmentLookupID, myFormat, view2D, myImage, color, levelCount, layercount);
			myAttachments[AttachmentLookupID] = Return;
			myInfo.Dispose(false);
			return Return;
		}

		Image myImage;
		internal FrameBufferAttachment CreateAttachment(string AttachmentLookupID, Format format, ImageViewType view2D, Image image, ImageAspectFlags color, uint levelCount, uint layercount)
		{
			myImage = image;
			if (Initialized) throw new Exception("Attempted to add attachment to initialized Framebuffer!");
			if (myAttachments.ContainsKey(AttachmentLookupID)) throw new Exception("A framebuffer attachment with ID: " + AttachmentLookupID + " was added to frame buffer twice.");
			var myAttache = new FrameBufferAttachment(AttachmentLookupID, image, view2D, format, color, levelCount, layercount);
			myAttachments[AttachmentLookupID] = myAttache;
			return myAttache;
		}



		public virtual void Initialize()
		{
			Initialized = true;
			if (myFramebufferInfo.RenderPass == null) throw new Exception("Attempted to create framebuffer without Renderpass!");
			myFramebufferInfo.Attachments = myAttachments.Values.ToArray();
			myFramebufferInfo.AttachmentCount = (uint)myFramebufferInfo.Attachments.Count();
			VulkanRenderer.SelectedLogicalDevice.CreateFramebuffer(this, myFramebufferInfo);
		}

		public void Dispose()
		{
			unsafe
			{
				Interoperator.NativeMethods.vkDestroyFramebuffer(VulkanRenderer.SelectedLogicalDevice.m, m, null);
			}
		}

		internal void SetRenderPass(EngineRenderer.RenderPass renderPass)
		{
			myFramebufferInfo.RenderPass = renderPass;
			Initialize();
		}
	}
}
