using Engine.Components;
using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;
using Engine.Renderer.Vulkan;
using Engine.Renderer.Vulkan.Descriptor_Sets;
using Engine.Renderer.Vulkan_Rendering.Buffers;
using EngineRenderer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
using RenderPass = EngineRenderer.RenderPass;

namespace Engine.Renderer.Vulkan_Rendering.RenderPasses.Deferred_Rendering
{
    public class GeometryPass : DeferredSubpass
    {
        GeometryBuffer myBackbuffer;


        Sampler mySample = new Sampler(Filter.Nearest, Filter.Nearest, SamplerMipmapMode.Linear, SamplerAddressMode.ClampToEdge, SamplerAddressMode.ClampToEdge, SamplerAddressMode.ClampToEdge,
                 0.0f, 1.0f, 0.0f, 1.0f, BorderColor.FloatOpaqueWhite);

        RenderPass OffscreenRenderPass;
       
        public GeometryPass(GeometryBuffer myBuffer, RenderPass OffscreenRenderPass, SecondaryCommandBufferPool mySecondaryCommandPool, ShaderManager myShaderManager) : base(mySecondaryCommandPool, "GeometryPass")
        {
            this.myShaderManager = myShaderManager;
            myRenderpassBeginInfo = new RenderPassBeginInfo
            {
                ClearValueCount = 4,
                ClearValues = new ClearValue[]
                {
                    new ClearValue { Color = new ClearColorValue(Color.AliceBlue) }, //TODO: Always remember to match these!
                    new ClearValue { Color = new ClearColorValue(Color.AliceBlue) }, //TODO: Always remember to match these!
                    new ClearValue { Color = new ClearColorValue(Color.AliceBlue) }, //TODO: Always remember to match these!
                    new ClearValue { DepthStencil = new ClearDepthStencilValue(){ Depth = 1.0f, Stencil = 0} }
                },
                RenderPass = OffscreenRenderPass,
                RenderArea = new Rect2D { Extent = new Extent2D { Width = VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent.Width, Height = VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent.Height }, Offset = new Offset2D { X = 0, Y = 0 } },
            };
            myBackbuffer = myBuffer;
            myRenderpassBeginInfo.Framebuffer = myBackbuffer;
            String[] ShaderNames = { "GBufferPassVertex", "GBufferPassFragment" };
            Tuple<PipelineLayout, Pipeline> myPipelineDefault = myShaderManager.CreatePipeline(OffscreenRenderPass, ShaderNames, out ExpectedModelSet);
            myActivePipelines.Add("Default", myPipelineDefault);
            this.OffscreenRenderPass = OffscreenRenderPass;


        }
        CommandBufferBeginInfo SecondaryBufferBeginInfo = new CommandBufferBeginInfo() { Flags = CommandBufferUsageFlags.OneTimeSubmit | CommandBufferUsageFlags.RenderPassContinue };
        
        public ResourceSet ExpectedModelSet;
        private ShaderManager myShaderManager;
        private RenderPassBeginInfo myRenderpassBeginInfo;
        ConcurrentDictionary<String, List<CommandBuffer>> myDictionary = new ConcurrentDictionary<string, List<CommandBuffer>>();
        public override void Draw(ResourceSet myResult,VertexBuffer<float> myVertexBuffer, IndexBuffer<UInt32> myIndexBuffer, String PipelineID)
        {
            CommandBuffer myBuffer = base.GetNext();//Synchronized for threading
            myBuffer.Begin(SecondaryBufferBeginInfo);
            myBuffer.CmdBindDescriptorSets(PipelineBindPoint.Graphics, myActivePipelines[PipelineID].Item1, 0, myResult.myDSet, null);
            myBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, myActivePipelines[PipelineID].Item2);
            myBuffer.CmdBindVertexBuffer(0, myVertexBuffer, 0);
            myBuffer.CmdBindIndexBuffer(myIndexBuffer, 0, IndexType.Uint32); //TODO: Fix this draw indexed and what about instancing
            myBuffer.CmdDrawIndexed(myIndexBuffer.Count, 1, 0, 0, 0); //TODO: Behind the scenes sort by pipeline ID
            myBuffer.End();
            if (!myDictionary.ContainsKey(PipelineID))
            {
                myDictionary[PipelineID] = new List<CommandBuffer>();
            }
            myDictionary[PipelineID].Add(myBuffer);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void ExecutePass(CommandBuffer renderPassPrimaryBuffer)
        {
            renderPassPrimaryBuffer.CmdDebugMarkerBeginEXT(myDebugMarker);
            renderPassPrimaryBuffer.CmdNextSubpass(SubpassContents.SecondaryCommandBuffers);
            var Result = GetCommandBuffersForRendering();
            renderPassPrimaryBuffer.CmdExecuteCommands(Result);
            renderPassPrimaryBuffer.CmdDebugMarkerEndEXT();
        }

        public GeometryBuffer GetGBuffer()
        {
           return myBackbuffer;
        }

        protected override CommandBuffer[] GetCommandBuffersForRendering()
        {
            List<CommandBuffer> myBuffers = new List<CommandBuffer>();
            foreach(var A in myDictionary)
            {
                if(A.Value.Count != 0)
                {
                    myBuffers.AddRange(A.Value);
                    A.Value.Clear();
                }
            }
            return myBuffers.ToArray();
        }

        public Tuple<string, DescriptorImageInfo>[] GetGBufferDescriptorImageInfo()
        {
            var myAttachments = this.myBackbuffer.GetAttachments();
            Tuple<string, DescriptorImageInfo>[] myDescriptorImages = new Tuple<string, DescriptorImageInfo>[myAttachments.Count];
            int lastInfoFilled = 0;
            foreach(FrameBufferAttachment A in myAttachments)
            {
                var myDescriptor = new DescriptorImageInfo();
                myDescriptor.ImageView = A;
                myDescriptor.ImageLayout = ImageLayout.ShaderReadOnlyOptimal;
                myDescriptor.Sampler = mySample;
                myDescriptorImages[lastInfoFilled] = new Tuple<string, DescriptorImageInfo>(A.Name, myDescriptor);
                lastInfoFilled++;
            }
            return myDescriptorImages;
        }
    }
}
