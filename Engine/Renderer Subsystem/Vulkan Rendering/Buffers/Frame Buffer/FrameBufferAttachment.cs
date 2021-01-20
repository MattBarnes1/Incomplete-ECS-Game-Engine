using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vulkan
{
    public class FrameBufferAttachment : ImageView, IDisposable
    {

		public FrameBufferAttachment(ImageViewCreateInfo myViewCreateInfo)
        {
            VulkanRenderer.SelectedLogicalDevice.CreateImageView(this, myViewCreateInfo);

        }

		

        public FrameBufferAttachment(String Name, Image image, ImageViewType view2D, Format format, ImageAspectFlags color, uint levelCount, uint layercount)
		{

          


            this.Name = Name;
			var viewCreateInfo = new ImageViewCreateInfo
			{
				Image = image,
				ViewType = view2D,
				Format = format,
				SubresourceRange = new ImageSubresourceRange
				{
					AspectMask = color,
					LevelCount = levelCount,
					LayerCount = layercount
				}
			};
			
			VulkanRenderer.SelectedLogicalDevice.CreateImageView(this, viewCreateInfo);
			viewCreateInfo.Dispose();
		}

		public String Name { get; }
		public AttachmentReference AttachmentReference { get; internal set; }
		public AttachmentDescription AttachmentDescription { get; internal set; }

		public void Dispose()
		{
			VulkanRenderer.SelectedLogicalDevice.DestroyImageView(this);
		}
	}
}
