using Vulkan;

namespace Engine.Renderer.Vulkan.Descriptor_Sets
{
    public class DescriptorInformation
    {
        public ShaderStageFlags ShaderStage { get; set; }
        public uint Binding { get; }
        public DescriptorType myType { get; }
        public uint DescriptorCount { get; }
        public DescriptorInformation(uint binding, DescriptorType uniformBuffer, ShaderStageFlags myStage, uint descriptorCount)
        {
            this.ShaderStage = myStage;
            Binding = binding;
            myType = uniformBuffer;
            DescriptorCount = descriptorCount;
        }
    }
}
