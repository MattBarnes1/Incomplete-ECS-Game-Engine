using Engine.Data_Types.BufferPool;
using Engine.Renderer.Vulkan;
using EngineRenderer;
using EngineRenderer.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace EngineRenderer
{
    public class SecondaryCommandBufferPool : BufferPool<CommandBuffer>
    {

        public SecondaryCommandBufferPool(VulkanLogicalDevice vulkanDevice, uint FamilyIndex, int commandBufferCount)
        {
            this.LogicalDevice = vulkanDevice;

            var createPoolInfo = new CommandPoolCreateInfo { Flags = CommandPoolCreateFlags.ResetCommandBuffer, QueueFamilyIndex = FamilyIndex  };
            this.CommandPool = LogicalDevice.CreateCommandPool(createPoolInfo);
            AllocateBuffers(commandBufferCount);
        }


        private VulkanLogicalDevice LogicalDevice;
        private CommandPool CommandPool;

        public override void SwapQueueResetBehaviour(CommandBuffer A)
        {
            A.Reset();
        }
        protected override void AllocateBuffers(int v)
        {
            var commandBufferAllocateInfo = new CommandBufferAllocateInfo
            {
                Level = CommandBufferLevel.Secondary,
                CommandPool = this.CommandPool,
                CommandBufferCount = (uint)v
            };
            myFreeBuffers.AddRange(LogicalDevice.AllocateCommandBuffers(commandBufferAllocateInfo));
        }
    }
}
