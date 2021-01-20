using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
using Interoperator = Vulkan;
namespace Engine.Renderer.Vulkan_Rendering
{
    public class Sampler : Interoperator.Sampler
    {

        public Sampler(Filter magFilter, Filter minFilter, SamplerMipmapMode mipmapMode, SamplerAddressMode addressModeU, SamplerAddressMode addressModeV, SamplerAddressMode addressModeW, float mipLodBias, float maxAnisotropy, float minLod, float maxLod, BorderColor aBorderColor)
        {
            SamplerCreateInfo sampler = new SamplerCreateInfo();
            sampler.MagFilter = magFilter;
            sampler.MinFilter = minFilter;
            sampler.MipmapMode = mipmapMode;
            sampler.AddressModeU = addressModeU;
            sampler.AddressModeV = addressModeV;
            sampler.AddressModeW = addressModeW;
            sampler.MipLodBias = mipLodBias;
            sampler.MaxAnisotropy = maxAnisotropy;
            sampler.MinLod = minLod;
            sampler.MaxLod = maxLod;
            sampler.BorderColor = aBorderColor;
            VulkanRenderer.SelectedLogicalDevice.CreateSampler(this, sampler);
        }
    }
}
