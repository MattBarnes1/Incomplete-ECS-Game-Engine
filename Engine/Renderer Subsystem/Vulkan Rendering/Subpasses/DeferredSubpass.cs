using Engine.Components;
using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;
using Engine.Renderer.Vulkan;
using Engine.Renderer.Vulkan.Descriptor_Sets;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer.Vulkan_Rendering.RenderPasses
{
    public abstract class DeferredSubpass
    {
        protected Dictionary<String, Tuple<PipelineLayout, Pipeline>> myActivePipelines = new Dictionary<string, Tuple<PipelineLayout, Pipeline>>();
        protected DebugMarkerMarkerInfoExt myDebugMarker;
        SecondaryCommandBufferPool mySecondaryCommandPool;
        List<CommandBuffer> myUsedBuffers = new List<CommandBuffer>();
        public DeferredSubpass(SecondaryCommandBufferPool mySecondaryCommandPool, String Name)
        {
            myDebugMarker = new DebugMarkerMarkerInfoExt() { MarkerName = Name };
            this.mySecondaryCommandPool = mySecondaryCommandPool;
        }

        protected CommandBuffer GetNext()
        {
            return mySecondaryCommandPool.GetNext();
        }

        public abstract void Draw(ResourceSet myResult, VertexBuffer<float> myVertexBuffer, IndexBuffer<UInt32> myIndexBuffer, String PipelineID);

        public virtual void ExecutePass(CommandBuffer renderPassPrimaryBuffer)
        {
            renderPassPrimaryBuffer.CmdDebugMarkerBeginEXT(myDebugMarker);
            renderPassPrimaryBuffer.CmdNextSubpass(SubpassContents.SecondaryCommandBuffers);
            var Result = GetCommandBuffersForRendering();
            renderPassPrimaryBuffer.CmdExecuteCommands(Result);
            renderPassPrimaryBuffer.CmdDebugMarkerEndEXT();
        }
        public virtual SubpassDescription GetSubpassDescription()
        {

            throw new NotImplementedException();
        }
        protected abstract CommandBuffer[] GetCommandBuffersForRendering();

    }
}
