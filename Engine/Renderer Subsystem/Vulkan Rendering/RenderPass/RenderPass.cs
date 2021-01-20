using Engine.Renderer.Vulkan;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
using Interop = Vulkan;


namespace EngineRenderer
{
    public class RenderPass : Interop.RenderPass
    {
		public RenderPass()
		{
            var anAttachmentDescription = new Interop.AttachmentDescription
            {
                Format = VulkanRenderer.Surface.SelectedSurfaceFormat.Format,
                Samples = Interop.SampleCountFlags.Count1,
                LoadOp = Interop.AttachmentLoadOp.Clear,
                StoreOp = Interop.AttachmentStoreOp.Store,
                StencilLoadOp = Interop.AttachmentLoadOp.DontCare,
                StencilStoreOp = Interop.AttachmentStoreOp.DontCare,
                InitialLayout = Interop.ImageLayout.Undefined,
                FinalLayout = Interop.ImageLayout.PresentSrcKHR
            };
            var anAttachmentReference = new Interop.AttachmentReference { Layout = Interop.ImageLayout.ColorAttachmentOptimal };
            var aSubpassDescription = new Interop.SubpassDescription
            {
                PipelineBindPoint = Interop.PipelineBindPoint.Graphics,
                ColorAttachments = new Interop.AttachmentReference[] { anAttachmentReference }
            };

            var aRenderPassCreateInfo = new Interop.RenderPassCreateInfo
            {
                Attachments = new Interop.AttachmentDescription[] { anAttachmentDescription },
                Subpasses = new Interop.SubpassDescription[] { aSubpassDescription }
            };
            CreateRenderPass(aRenderPassCreateInfo);
        }


        public RenderPass(FrameBuffer myFrameBuffer, SubPass[] mySubpasses = null)
        {
             List<AttachmentDescription> GBufferSupportedTypes = new List<AttachmentDescription>();
            List<AttachmentReference> attachmentColorReferences = new List<AttachmentReference>();
            AttachmentReference myDepthBuffer;
            SubpassDescription GeometryBufferSubpassDescription = new SubpassDescription() { PipelineBindPoint = PipelineBindPoint.Graphics };

            foreach (var Attachment in myFrameBuffer.GetAttachments())
            {
                GBufferSupportedTypes.Add(Attachment.AttachmentDescription);
                if (Attachment.AttachmentReference.Layout != ImageLayout.DepthStencilAttachmentOptimal)
                {
                    attachmentColorReferences.Add(Attachment.AttachmentReference);
                }
                else
                {
                    GeometryBufferSubpassDescription.DepthStencilAttachment = Attachment.AttachmentReference;
                }
            }
            GeometryBufferSubpassDescription.ColorAttachments = attachmentColorReferences.ToArray();


            //Above is for the Geometry Buffer to intialize.
            uint starting = 1;
            uint ending = 2;
            List<SubpassDependency> myIncomingDependencies = new List<SubpassDependency>();
            List<SubpassDescription> mySubpassDescriptions = new List<SubpassDescription>();
            if(mySubpasses != null)
            {
                for (int i = 0; i < mySubpasses.Length; i++)
                {
                    myIncomingDependencies.AddRange(mySubpasses[i].GetSubpassDependencies());
                    mySubpassDescriptions.Add(mySubpasses[i].GetSubpassDescription());
                };
            }

            var aRenderPassCreateInfo = new RenderPassCreateInfo
            {
                Attachments = GBufferSupportedTypes.ToArray(),
                Subpasses = mySubpassDescriptions.ToArray(),
                Dependencies = myIncomingDependencies.ToArray()
            };
            myFrameBuffer.SetRenderPass(this);
            CreateRenderPass(aRenderPassCreateInfo);
        }

		private void CreateRenderPass(Interop.RenderPassCreateInfo pCreateInfo)
		{
            Interop.Result result;
			unsafe
			{

				fixed (UInt64* ptrpRenderPass = &this.m)
				{
					result = Interop.Interop.NativeMethods.vkCreateRenderPass(VulkanRenderer.SelectedLogicalDevice.m, pCreateInfo != null ? pCreateInfo.m : (Interop.Interop.RenderPassCreateInfo*)default(IntPtr), null, ptrpRenderPass);
				}
				if (result != Interop.Result.Success)
					throw new Interop.ResultException(result);
			}
		}







	}
}
