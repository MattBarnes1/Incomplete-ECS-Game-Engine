using Engine.Components;
using Engine.Renderer.Vulkan;
using Engine.Renderer.Vulkan_Rendering.Buffers;
using Engine.Renderer.Vulkan_Rendering.RenderPasses.Deferred_Rendering;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
namespace Engine.Renderer.Vulkan
{
    public abstract class SubPass
    {
        protected Dictionary<String, Tuple<PipelineLayout, Pipeline>> myActivePipelines = new Dictionary<string, Tuple<PipelineLayout, Pipeline>>();
        public DebugMarkerMarkerInfoExt myDebugMarker;
        public SubPass(String PassName)
        {
            myDebugMarker = new DebugMarkerMarkerInfoExt() { MarkerName = PassName };
        }

        public ref SubpassDependency StartingDependency { get { return ref StartDependency; } }
        public ref SubpassDependency EndingDependency { get { return ref EndDependency; } }

        private SubpassDependency StartDependency = new SubpassDependency() //Start dependency
        {
            SrcSubpass = uint.MaxValue,
            DstSubpass = 0,
            SrcStageMask = PipelineStageFlags.BottomOfPipe,
            DstStageMask = PipelineStageFlags.ColorAttachmentOutput,
            DstAccessMask = AccessFlags.ColorAttachmentRead | AccessFlags.ColorAttachmentRead,
            SrcAccessMask = AccessFlags.MemoryRead,
            DependencyFlags = DependencyFlags.ByRegion
        };

        private SubpassDependency EndDependency = new SubpassDependency() //Start dependency
        {
            SrcSubpass = 0,
            DstSubpass = uint.MaxValue,
            SrcStageMask = PipelineStageFlags.BottomOfPipe,
            DstStageMask = PipelineStageFlags.ColorAttachmentOutput,
            DstAccessMask = AccessFlags.ColorAttachmentRead | AccessFlags.ColorAttachmentRead,
            SrcAccessMask = AccessFlags.MemoryRead,
            DependencyFlags = DependencyFlags.ByRegion
        };

       

        public void SetSubpassStart(uint starting)
        {
            if(starting == uint.MaxValue)
            {
                StartDependency.SrcSubpass = uint.MaxValue;//Not zero    VK_SUBPASS_EXTERNAL is just ~0 so maxvalue from the turn over.
                StartDependency.DstSubpass = 0;
                EndDependency.SrcSubpass = 0;
            } 
            else
            {
                StartDependency.SrcSubpass = starting;//Not zero    VK_SUBPASS_EXTERNAL is just ~0 so maxvalue from the turn over.
                StartDependency.DstSubpass = starting + 1;
                EndDependency.SrcSubpass = starting + 1;                
            }
        }

        public void SetSubpassEnd(uint value)
        {
            EndDependency.DstSubpass = value;
        }

        public SubpassDependency[] GetSubpassDependencies()
        {
            return new SubpassDependency[] { StartDependency, EndDependency };
        }

        SubpassDescription myDescription = new SubpassDescription();
        
        public abstract SubpassDescription GetSubpassDescription();
        protected abstract void DoPass(CommandBuffer renderPassPrimaryBuffer);
        public virtual void ExecutePass(CommandBuffer renderPassPrimaryBuffer)
        {
            renderPassPrimaryBuffer.CmdNextSubpass(SubpassContents.Inline);
            renderPassPrimaryBuffer.CmdDebugMarkerBeginEXT(myDebugMarker);
            DoPass(renderPassPrimaryBuffer);
            renderPassPrimaryBuffer.CmdDebugMarkerEndEXT();
        }

        GeometryPass myGBufferPass;

        internal void GetResourceSet()
        {
            throw new NotImplementedException();
        }


        //TODO:https://www.khronos.org/assets/uploads/developers/library/2016-vulkan-devu-seoul/6-Pipeline-Cache-Object.pdf

    }
}
