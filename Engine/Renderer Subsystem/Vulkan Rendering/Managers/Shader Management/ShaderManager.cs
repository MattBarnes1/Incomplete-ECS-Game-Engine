using Engine.Renderer.Vulkan.Descriptor_Sets;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer.Vulkan
{
    [TestReminder("Shader Manager", "General testing needed!")]
    public class ShaderManager
    {
        Dictionary<String, Shader> myShaders = new Dictionary<string, Shader>();
        //protected Pipeline myRenderpassPipeline;
        private PipelineCache myPipelineCache;
        //private PipelineLayoutCreateInfo pipelineLayoutCreateInfo;

        public DescriptorSetPoolManager DescriptorPool { get; private set; }

        public ShaderManager(DescriptorSetPoolManager myDescriptorPool)
        {
            this.DescriptorPool = myDescriptorPool;
            if (myPipelineCache != null) throw new Exception("Pipeline cache was initialized twice without prior instance being destroyed.");
            myPipelineCache = VulkanRenderer.SelectedLogicalDevice.CreatePipelineCache(new PipelineCacheCreateInfo());
            LoadShaderFiles();
        }

        private void LoadShaderFiles()
        {
            Shader myShader = new Shader("GBufferPassVertex", "main", LoadResource("Engine.Renderer.Shaders.GBufferPassVertex.spv"), ShaderStageFlags.Vertex);
            myShader.Descriptor.AddDescriptor("MVPUniform", DescriptorType.UniformBuffer, 0);
            myShader.Descriptor.AddDescriptor("Position", DescriptorType.CombinedImageSampler, 1);
            myShader.Descriptor.AddDescriptor("Normal", DescriptorType.CombinedImageSampler, 2);
            myShader.Descriptor.AddDescriptor("Diffuse", DescriptorType.CombinedImageSampler, 3);
            myShader.Descriptor.AddDescriptor("Depth Buffer", DescriptorType.CombinedImageSampler, 4);
            myShaders.Add(myShader.ShaderName, myShader);

            Shader myFragShader = new Shader("GBufferPassFragment", "main", LoadResource("Engine.Renderer.Shaders.GBufferPassFrag.spv"), ShaderStageFlags.Fragment);
            myShaders.Add(myFragShader.ShaderName, myFragShader);
            myFragShader.Descriptor.AddDescriptor("ColorSampler", DescriptorType.CombinedImageSampler, 1);
            myFragShader.Descriptor.AddDescriptor("NormalSampler", DescriptorType.CombinedImageSampler, 2);

            Shader myOffscreenVertShader = new Shader("OffscreenToOnScreenVert", "main", LoadResource("Engine.Renderer.Shaders.OffscreenToOnScreenVert.spv"), ShaderStageFlags.Vertex);
            myShaders.Add(myOffscreenVertShader.ShaderName, myOffscreenVertShader);
            myOffscreenVertShader.Descriptor.AddDescriptor("MVPUniform", DescriptorType.UniformBuffer, 0);
            myOffscreenVertShader.Descriptor.AddDescriptor("Position", DescriptorType.CombinedImageSampler, 1);
            myOffscreenVertShader.Descriptor.AddDescriptor("Normal", DescriptorType.CombinedImageSampler, 2);
            myOffscreenVertShader.Descriptor.AddDescriptor("Diffuse", DescriptorType.CombinedImageSampler, 3);
            myOffscreenVertShader.Descriptor.AddDescriptor("Depth Buffer", DescriptorType.CombinedImageSampler, 4);
            myOffscreenVertShader.VertexState = new VertexBuilder();

            Shader myOffscreenShader = new Shader("OffscreenToOnScreenFrag", "main", LoadResource("Engine.Renderer.Shaders.OffscreenToOnScreenFrag.spv"), ShaderStageFlags.Fragment);
            myShaders.Add(myOffscreenShader.ShaderName, myOffscreenShader);
            myOffscreenShader.Descriptor.AddDescriptor("PositionSampler", DescriptorType.CombinedImageSampler,  1);
            myOffscreenShader.Descriptor.AddDescriptor("DiffuseSampler", DescriptorType.CombinedImageSampler,  3);
            myOffscreenShader.Descriptor.AddDescriptor("NormalSampler", DescriptorType.CombinedImageSampler,  2);


            return;
        }


        public byte[] LoadResource(string name)
        {
            String[] Names = this.GetType().Assembly.GetManifestResourceNames();
            System.IO.Stream stream = this.GetType().Assembly.GetManifestResourceStream(name);
            var res = this.GetType().Assembly.GetManifestResourceNames();
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);

            return bytes;
        }

        internal Tuple<PipelineLayout, Pipeline> CreatePipeline(EngineRenderer.RenderPass aPass, string[] shaderNames, out Descriptor_Sets.ResourceSet myDeferredRendererDescriptorSet)
        {
            PipelineVertexInputStateCreateInfo myState = null;
            List<PipelineShaderStageCreateInfo> myStages = new List<PipelineShaderStageCreateInfo>();
            myDeferredRendererDescriptorSet = new Descriptor_Sets.ResourceSet();
            foreach (String A in shaderNames)
            {
                Shader Val = myShaders[A];
                myDeferredRendererDescriptorSet.MergeFrom(Val.Descriptor);
                myStages.Add(new PipelineShaderStageCreateInfo()
                {
                    Stage = Val.ShaderStage,
                    Module = VulkanRenderer.SelectedLogicalDevice.CreateShaderModule(Val.ShaderBytecode),
                    Name = Val.EntryName,
                });;
                if(Val.ShaderStage == ShaderStageFlags.Vertex)
                {
                    if (Val.VertexState != null)
                    {
                        myState = Val.VertexState.GetStateCreateInfo();
                    } else
                    {
                        myState = new VertexBuilder().GetDefaultStateCreateInfo();
                    }
                }
            }
#if DEBUG
            if(!ValidateShaderStages(myStages))
            {
                throw new Exception("Shader stages either had duplicates or nothing at all!");
            }
#endif
          var myLayout = CreatePipelineLayout(myDeferredRendererDescriptorSet.GetDescriptorSetLayout());

            return new Tuple<PipelineLayout, Pipeline>(myLayout, CreatePipelineFromData(aPass, myState, myLayout, myStages));
        }


        private Pipeline CreatePipelineFromData(EngineRenderer.RenderPass aPass, PipelineVertexInputStateCreateInfo myState, PipelineLayout myLayout, List<PipelineShaderStageCreateInfo> myStages)
        {
            var scissor = new Rect2D { Extent = VulkanRenderer.Surface.SurfaceCapabilities.CurrentExtent };
            var viewportCreateInfo = new PipelineViewportStateCreateInfo
            {
                Viewports = new Viewport[] { VulkanRenderer.Viewport },
                Scissors = new Rect2D[] { scissor }
            };
            
            var multisampleCreateInfo = new PipelineMultisampleStateCreateInfo
            {
                RasterizationSamples = SampleCountFlags.Count1
            };
            var colorBlendAttachmentState = new PipelineColorBlendAttachmentState
            {
                ColorWriteMask = ColorComponentFlags.R | ColorComponentFlags.G | ColorComponentFlags.B | ColorComponentFlags.A
            };

            var colorBlendStateCreatInfo = new PipelineColorBlendStateCreateInfo
            {
                LogicOp = LogicOp.Copy,
                Attachments = new PipelineColorBlendAttachmentState[] { colorBlendAttachmentState }
            };

            var rasterizationStateCreateInfo = new PipelineRasterizationStateCreateInfo
            {
                PolygonMode = PolygonMode.Fill,
                CullMode = CullModeFlags.None,
                FrontFace = FrontFace.Clockwise, //Todo: flip?
                LineWidth = 1.0f
            };

            var inputAssemblyStateCreateInfo = new PipelineInputAssemblyStateCreateInfo
            {
                Topology = PrimitiveTopology.TriangleList
            };


            var pipelineCreateInfo = new GraphicsPipelineCreateInfo
            {
                Layout = myLayout,
                ViewportState = viewportCreateInfo,
                Stages = myStages.ToArray(),
                MultisampleState = multisampleCreateInfo,
                ColorBlendState = colorBlendStateCreatInfo,
                RasterizationState = rasterizationStateCreateInfo,
                InputAssemblyState = inputAssemblyStateCreateInfo,
                VertexInputState = myState,// myBuilder.GetDefaultStateCreateInfo(),
                RenderPass = aPass,
                DepthStencilState = new PipelineDepthStencilStateCreateInfo()
                {
                    

                }
            };


            Debug.Assert(pipelineCreateInfo.Stages[0].Module.m == myStages[0].Module.m);
            var Result = VulkanRenderer.SelectedLogicalDevice.CreateGraphicsPipelines(myPipelineCache, new GraphicsPipelineCreateInfo[] { pipelineCreateInfo });
            pipelineCreateInfo.Dispose();
            inputAssemblyStateCreateInfo.Dispose();
            rasterizationStateCreateInfo.Dispose();
            colorBlendStateCreatInfo.Dispose();
            Debug.Assert(Result.Length == 1);
            return Result[0];
        }

        private bool ValidateShaderStages(List<PipelineShaderStageCreateInfo> myStages)
        {
            //TODO: 
            return true;



        }

        private PipelineLayout CreatePipelineLayout(DescriptorSetLayout descriptorSetLayout)
        {
            var pipelineLayoutCreateInfo = new PipelineLayoutCreateInfo
            {
                SetLayouts = new DescriptorSetLayout[] { descriptorSetLayout }
            };
            return VulkanRenderer.SelectedLogicalDevice.CreatePipelineLayout(pipelineLayoutCreateInfo);
        }

       
    

        /* */
    }
}
