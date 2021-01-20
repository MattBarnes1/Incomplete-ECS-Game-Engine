using Engine;
using Engine.DataTypes.Texture_Data;
using Engine.Renderer;
using EngineRenderer.Interfaces;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Vulkan;
using Engine.DataTypes;
using Engine.Components;
using System.Collections.Concurrent;
using Engine.Renderer.Vulkan;
using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;
using System.Numerics;
using Engine.Renderer.Vulkan_Rendering.RenderPasses;
using Engine.Low_Level.Vulkan.Classes_By_Me.Frame_Buffer_Pool;
using EngineRenderer.Exceptions.VULKAN;
using Engine.Renderer.Vulkan.Descriptor_Sets;
using EngineRenderer.Vulkan;
using Engine.ECS.Components;
using Engine.Utilities;
using Engine.Renderer.Vulkan_Rendering.Subpasses;
using Engine.Renderer.Vulkan_Rendering.RenderPasses.Deferred_Rendering;
using Engine.Renderer.Vulkan_Rendering.Buffers;

namespace EngineRenderer
{
    /// <summary>
    /// Holds information for graphics settings, manages the type of renderer we're using (deferred/forward).
    /// </summary>
    public partial class VulkanRenderer : IRenderingManager
    {
        public static LightingManager GlobalLightingManager { get; private set; }
        public static SurfaceKHR Surface { get; private set; }
        public static VulkanInstance Window { get; private set; }
        public static VulkanPhysicalDevice SelectedPhysicalDevice { get; private set; }
        public static VulkanLogicalDevice SelectedLogicalDevice { get; private set; }
        private Engine.Renderer.Vulkan.Queue GraphicsQueue { get; set; }
        public static Viewport Viewport { get; private set; }

        private SubPass[] mySubpasses;

        public static uint ActiveGraphicsFamilyQueueIndex { get; private set; }

        private SwapchainManager mySwapchain;
        public FrameBufferManager myFramebufferManager { get; private set; }
        public static DescriptorSetPoolManager myDescriptorPoolManager { get; private set; }


        CommandBuffer GBufferPrimaryCommandBuffer;
        CommandBufferInheritanceInfo myRenderPassInheritedBuffer;
        PrimaryCommandBufferPool PrimaryCommandPool;



        Dictionary<String, Tuple<PipelineLayout, Pipeline>> myActivePipelines = new Dictionary<string, Tuple<PipelineLayout, Pipeline>>();


        GeometryPass GeometryPass;

        DebugMarkerMarkerInfoExt EndOFDeferred = new DebugMarkerMarkerInfoExt { MarkerName = "Deferred Drawing to Screen" };
        Queue<SecondaryCommandBufferPool> myDeferredPassesCommandPoolQueue = new Queue<SecondaryCommandBufferPool>();


        ResourceSet myDeferredRendererDescriptorSet;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="LogicalDevice"></param>
        /// <param name="DescriptorPoolManager">Since it's possible for one device to support both the compute and the rendering, the VulkanInitializer will decide and send it to us. </param>
        /// <param name="PhysicalDevice"></param>
        /// <param name="MainGraphicsQueue"></param>
        /// <param name="mySettings"></param>
        public VulkanRenderer(LightingManager myManager, SurfaceKHR Surface, VulkanInstance myInstance, VulkanLogicalDevice LogicalDevice, DescriptorSetPoolManager DescriptorPoolManager, VulkanPhysicalDevice PhysicalDevice, Engine.Renderer.Vulkan.Queue MainGraphicsQueue, GraphicsSettings mySettings)
        {
            #region Variables from Initializer, Fences, Semaphores
            GlobalLightingManager = myManager;
            VulkanRenderer.Surface = Surface;
            VulkanRenderer.Window = myInstance;
            VulkanRenderer.SelectedLogicalDevice = LogicalDevice;
            VulkanRenderer.SelectedPhysicalDevice = PhysicalDevice;
            //For now we'll only support one main Graphics Queue that may or may not be used in other places like the Compute Shader. Depends on graphics card.
            GraphicsQueue = MainGraphicsQueue;
            ActiveGraphicsFamilyQueueIndex = GraphicsQueue.QueueFamilyIndex;
            //Creates Debug Camera
           
            this.ActiveCamera = new CameraComponent(); //TODO: Remove after debug phase is finally over.

            VulkanRenderer.Viewport = new Viewport
            {
                Width = mySettings.SCREEN_WIDTH,
                Height = mySettings.SCREEN_HEIGHT,
                MinDepth = 0,
                MaxDepth = 1.0f,
            };
            FenceCreateInfo fenceInfo = new FenceCreateInfo();
            DrawingFence = VulkanRenderer.SelectedLogicalDevice.CreateFence(fenceInfo);
            OffscreenRenderFinishedSemaphore = VulkanRenderer.SelectedLogicalDevice.CreateSemaphore(new SemaphoreCreateInfo());
            #endregion
            #region Vulkan Pools and Queues Initialization
            myDescriptorPoolManager = DescriptorPoolManager;
            myShaderManager = new ShaderManager(myDescriptorPoolManager);
            myFramebufferManager = new FrameBufferManager(mySettings.SCREEN_WIDTH, mySettings.SCREEN_HEIGHT);//TODO: Needed?
            myModelResourceManager = new ResourceSetManager();
            mySwapchain = new SwapchainManager(LogicalDevice, Surface.SelectedSurfaceFormat);
            PrimaryCommandPool = new PrimaryCommandBufferPool(LogicalDevice, GraphicsQueue.QueueFamilyIndex, 1);            
            int Count = LogicalDevice.QueueManager.GetFreeQueueCount(QueueFlags.Graphics);
            for (int i = 0; i < Count; i++)
            {
                myDeferredPassesCommandPoolQueue.Enqueue(new SecondaryCommandBufferPool(LogicalDevice, GraphicsQueue.QueueFamilyIndex, 30));
            }
            #endregion
            #region GBuffer, Subpass Initialization
            GeometryBuffer myGeometryBuffer = new GeometryBuffer();


            mySubpasses = GetSubpassesFromGraphics(mySettings);
            uint starting = 1;
            uint ending = 2;
            for (int i = 0; i < mySubpasses.Length; i++)
            {
                mySubpasses[i].SetSubpassStart(starting + 1);
                if (i + 1 >= mySubpasses.Length)
                    mySubpasses[i].SetSubpassEnd(UInt32.MaxValue);
                ending++;
                starting++;
            }
            
            var OffscreenRenderPass = new RenderPass(myGeometryBuffer, mySubpasses);

            GeometryPass = new GeometryPass(myGeometryBuffer, OffscreenRenderPass, myDeferredPassesCommandPoolQueue.Dequeue(), myShaderManager);
          
            #endregion

            #region Final Composition Render Setup
            OnScreenRenderPass = new RenderPass();// this.CreateFinalRenderPass(LogicalDevice);
            var Result = myShaderManager.CreatePipeline(OnScreenRenderPass, new String[] { "OffscreenToOnScreenVert", "OffscreenToOnScreenFrag" }, out myDeferredRendererDescriptorSet);//TODO: create method that returns resource set for shader of a given type.         
            DeferredRendererOutputPipe = Result.Item2;
            DeferredRendererOutputPipeLayout = Result.Item1;
            Tuple<String, DescriptorImageInfo>[] myDescriptorInfo = GeometryPass.GetGBufferDescriptorImageInfo();
            if(mySubpasses.Length != 0)
            {
                foreach (var A in myDescriptorInfo)
                {
                    foreach (SubPass B in mySubpasses)
                    {
                        //ResourceSet mySet = B.GetResourceSet();
                        // mySet.Write(A.Item1, A.Item2);
                    }
                }
            }
            //Tell our deferred renderer that we can
            foreach (var A in myDescriptorInfo)
            {
                myDeferredRendererDescriptorSet.Write(A.Item1, A.Item2);
            }
            CreateFinalQuadRenderingSpace();
            #endregion
        }


        VertexBuffer<Vertex> myScreenQuadVertexBuffer;
        IndexBuffer<uint> myScreenQuadIndexBuffer;
        private unsafe void CreateFinalQuadRenderingSpace()
        {
            if (myScreenQuadIndexBuffer != null) return; //So we don't recreate this.
            myScreenQuadIndexBuffer = new IndexBuffer<uint>(new uint[] { 0, 1, 2, 2, 3, 0 });
            Vertex[] ScreenQuadVertices = new Vertex[]
            {
                new Vertex(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),  new Vector3(1.0f, 1.0f, 1.0f)),
                new Vertex(new Vector3(0, 1.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),  new Vector3(1.0f, 1.0f, 1.0f)),
                new Vertex(new Vector3(0, 0, 0.0f), new Vector2(0f, 0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),  new Vector3(1.0f, 1.0f, 1.0f)),
                new Vertex(new Vector3(1.0f, 0, 0.0f), new Vector2(1.0f,0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),  new Vector3(1.0f, 1.0f, 1.0f)),
            };
            myScreenQuadVertexBuffer = new VertexBuffer<Vertex>(ScreenQuadVertices);
        }
        private SubPass[] GetSubpassesFromGraphics(GraphicsSettings mySettings)
        {
            List<SubPass> mySubpassesToReturn = new List<SubPass>();

            switch (mySettings.myAliasType)
            {
                case ANTIALIAS.NONE:

                    break;
                case ANTIALIAS.SSAO:
                    mySubpassesToReturn.Add(new SSAOSubpass());
                    break;
                case ANTIALIAS.MSSAO:

                    break;
            }

            return mySubpassesToReturn.ToArray();
        }

        internal Fence GetFinshedFence()
        {
            return DrawingFence;
        }

        Fence ComputeFence;

        internal void SetComputeFence(Fence p)
        {
            ComputeFence = p;
        }

        public CameraComponent ActiveCamera { get; private set; }


        private ShaderManager myShaderManager;


        CommandBuffer RendererFinalCompositionBuffer;

        public void RenderStart()
        {
            ProjectionPerspective = Matrix4x4.CreatePerspective(0.785398f, VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent.Width / VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent.Height, 0.1f, 100.0f);
            LookAtMatrix = ActiveCamera.GetLookAtMatrix();
            VulkanRenderer.SelectedLogicalDevice.WaitForFence(ComputeFence, true, 100000000);
            activeSwapChain = mySwapchain.GetRenderPassBegin();
            PrimaryCommandPool.ResetQueues();
            GBufferPrimaryCommandBuffer = PrimaryCommandPool.GetNext();//Creates the primary command buffer for this render pass
            RendererFinalCompositionBuffer = PrimaryCommandPool.GetNext();



            VulkanRenderer.SelectedLogicalDevice.ResetFence(DrawingFence);

        }

        public void RenderEnd()
        {
            GeometryPass.ExecutePass(GBufferPrimaryCommandBuffer);
            foreach (SubPass A in mySubpasses)
            {
                A.ExecutePass(GBufferPrimaryCommandBuffer);
            }
            GBufferPrimaryCommandBuffer.CmdEndRenderPass();
            GBufferPrimaryCommandBuffer.End();


            var submitInfo = new SubmitInfo //TODO: Move to Graphics queue
            {
                WaitSemaphores = new Semaphore[] { mySwapchain.SwapchainDrawCompleteSemaphore },
                WaitDstStageMask = new PipelineStageFlags[] { PipelineStageFlags.AllGraphics },
                CommandBufferCount = (uint)1,
                CommandBuffers = new CommandBuffer[] { GBufferPrimaryCommandBuffer },
                SignalSemaphores = new Semaphore[] { OffscreenRenderFinishedSemaphore } //TODO: create this semaphore
            };
            GraphicsQueue.Submit(submitInfo);
            
            
            
            
            /* Draws the G-buffer to the screen quad*/




            RendererFinalCompositionBuffer.CmdDebugMarkerBeginEXT(EndOFDeferred);
            RendererFinalCompositionBuffer.CmdBeginRenderPass(this.activeSwapChain, SubpassContents.Inline);
            RendererFinalCompositionBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, this.DeferredRendererOutputPipe);
            RendererFinalCompositionBuffer.CmdBindDescriptorSets(PipelineBindPoint.Graphics, this.DeferredRendererOutputPipeLayout, 0, myDeferredRendererDescriptorSet.myDSet, null);
            RendererFinalCompositionBuffer.CmdBindIndexBuffer(this.myScreenQuadIndexBuffer, this.myScreenQuadIndexBuffer.SizeOfBuffer, IndexType.Uint32);
            RendererFinalCompositionBuffer.CmdBindVertexBuffer(0, this.myScreenQuadVertexBuffer, this.myScreenQuadIndexBuffer.SizeOfBuffer);
            RendererFinalCompositionBuffer.CmdDrawIndexed(this.myScreenQuadIndexBuffer.Count, 1, 0, 0, 0);
            RendererFinalCompositionBuffer.CmdEndRenderPass();
            RendererFinalCompositionBuffer.CmdDebugMarkerEndEXT();
            RendererFinalCompositionBuffer.End();


            var submitInfo1 = new SubmitInfo //TODO: Move to Graphics queue
            {
                WaitSemaphores = new Semaphore[] { OffscreenRenderFinishedSemaphore },
                WaitDstStageMask = new PipelineStageFlags[] { PipelineStageFlags.AllGraphics },
                CommandBufferCount = (uint)1,
                CommandBuffers = new CommandBuffer[] { RendererFinalCompositionBuffer },
            };

            GraphicsQueue.Submit(submitInfo1, DrawingFence);

            submitInfo.Dispose();
            submitInfo1.Dispose();//TODO: Verify this is a good thing or not.
            //TODO: RenderTarget Processing.
            try
            {
                GraphicsQueue.PresentKHR(mySwapchain.SwapChainPresentInfo);
            }
            catch (VULKAN_ERROR_OUT_OF_DATE_KHR E)
            {
                RebuildSwapChain();
            }
            catch (VULKAN_ERROR_SUBOPTIMAL_KHR E)
            {
                RebuildSwapChain();
            }
            mySwapchain.SwapBuffer();
        }

        private void RebuildSwapChain()
        {
            mySwapchain.Dispose();
            //myRenderPassManager.Dispose();
            mySwapchain = new SwapchainManager(SelectedLogicalDevice, Surface.SelectedSurfaceFormat);


        }




        ResourceSetManager myModelResourceManager;
        Matrix4x4 ProjectionPerspective;
        Matrix4x4 LookAtMatrix;
        private Fence DrawingFence;
        private RenderPass OnScreenRenderPass;
        private Pipeline DeferredRendererOutputPipe;
        private PipelineLayout DeferredRendererOutputPipeLayout;
        private RenderPassBeginInfo activeSwapChain;

        public Semaphore OffscreenRenderFinishedSemaphore { get; private set; }

        public void DrawWorldSpace(uint ID, ModelComponent myModel, TextureComponent myTexture, TransformComponent myTransform)
        {
            if(this.ActiveCamera.isInViewingFustrum(myTransform.WorldPosition))
            {
                MVPUniformBuffer areaBuffer = new MVPUniformBuffer();
                areaBuffer.View = LookAtMatrix; //
                areaBuffer.Proj = ProjectionPerspective; //
                areaBuffer.Model = Matrix4x4.CreateTranslation(myTransform.WorldPosition);
                ResourceSet myResult;
                myModelResourceManager.TryGetOrCreate<ModelComponent>(ID, out myResult);
                myResult.Write("MVPUniform", areaBuffer);
                if (!myModel.hasTransparency)
                    GeometryPass.Draw(myResult, myModel.myVertexBuffer, myModel.myIndexBuffer, myModel.PipelineID);
                else
                {
                    //Iterate through the model's submeshes, find the transparency and delay it.
                    throw new Exception("Transparency not supported as of yet!");
                }
            }
        }








        public void SetCamera(CameraComponent cameraComponent)
        {
            this.ActiveCamera = cameraComponent;
        }




    }
}