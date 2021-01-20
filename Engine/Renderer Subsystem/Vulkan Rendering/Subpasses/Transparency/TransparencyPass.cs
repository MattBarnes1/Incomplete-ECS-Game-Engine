using Engine.Components;
using Engine.Renderer.Vulkan.Descriptor_Sets;
using Engine.Renderer.Vulkan_Rendering.RenderPasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
using EngineRenderer;
using Engine.Renderer.Vulkan;
using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;

namespace Engine.Renderer.Vulkan_Rendering.Subpasses.Transparency
{
    public class TransparencyPass : DeferredSubpass
    {
        public TransparencyPass(SecondaryCommandBufferPool mySecondaryCommandPool, string Name) : base(mySecondaryCommandPool, Name)
        {
        }

        public override void ExecutePass(CommandBuffer renderPassPrimaryBuffer)
        {

        }

        public override void Draw(ResourceSet myResult, VertexBuffer<float> myVertexBuffer, IndexBuffer<uint> myIndexBuffer, string PipelineID)
        {
            Tuple<ResourceSet, VertexBuffer<float>, IndexBuffer<uint>, string> myTuple;
        }

        protected override CommandBuffer[] GetCommandBuffersForRendering()
        {
            throw new NotImplementedException();
        }
    }
}
