using Engine.Renderer.Vulkan.Descriptor_Sets;
using System;
using Vulkan;

namespace EngineRenderer
{
    internal class ResourceSetManager
    {
        SubResourceSet myModelTemplate = new SubResourceSet(ShaderStageFlags.Vertex);
        public ResourceSetManager()
        {
            myModelTemplate.AddDescriptor("MVPUniform", DescriptorType.UniformBuffer, 0); //TODO: allocating in batches.
        }
        public ResourceSet CreateDefaultModelResources()
        {
            ResourceSet mySet = new ResourceSet();
            mySet.MergeFrom(myModelTemplate);
            mySet.Allocate();
            return mySet;
        }
        internal void TryGetOrCreate<T>(uint iD, out ResourceSet myResult)
        {
            throw new NotImplementedException();
        }
    }
}