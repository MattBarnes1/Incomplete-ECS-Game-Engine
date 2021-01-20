using Engine.Renderer.Vulkan;
using Engine.Renderer.Vulkan.Descriptor_Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine
{
    [TestReminder("Shader", "Write initial class.")]
    public class Shader
    {
        public int DescriptorCount { get; internal set; }
        public SubResourceSet Descriptor { get; internal set; }



        public byte[] ShaderBytecode { get; }
        public String ShaderName { get; }
        public ShaderStageFlags ShaderStage { get;}
        public string EntryName { get; }
        public VertexBuilder VertexState { get; internal set; }

        public Shader(string ShaderID, string EntryPoint, byte[] ShaderBytecode, ShaderStageFlags shaderType)
        {
            Descriptor = new SubResourceSet(shaderType);
            EntryName = EntryPoint;
            this.ShaderName = ShaderID;
            this.ShaderBytecode = ShaderBytecode;
            ShaderStage = shaderType;
        }

    }
}
