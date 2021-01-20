using Engine.Data_Types;
using Engine.Data_Types.BufferPool;
using Engine.Utilities;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer
{
    public class PrimaryCommandBufferPool : BufferPool<CommandBuffer>
    {
        protected override void AllocateBuffers(int v)
        {
            // var createPoolInfo = new CommandPoolCreateInfo { };
            //CommandPool = LogicalDevice.CreateCommandPool(createPoolInfo);
            var commandBufferAllocateInfo = new CommandBufferAllocateInfo
            {
                Level = CommandBufferLevel.Primary,
                CommandPool = this.CommandPool,
                CommandBufferCount = (uint)v
            };
            base.myFreeBuffers.AddRange(LogicalDevice.AllocateCommandBuffers(commandBufferAllocateInfo));
        }

        public override void SwapQueueResetBehaviour(CommandBuffer A)
        {
            A.Reset();
        }

        public Device LogicalDevice { get; private set; }
        public CommandPool CommandPool { get; private set; }

        public PrimaryCommandBufferPool(Device vulkanDevice, uint FamilyIndex, int commandBufferCount)
        {
            this.LogicalDevice = vulkanDevice;
            var createPoolInfo = new CommandPoolCreateInfo { Flags = CommandPoolCreateFlags.ResetCommandBuffer, QueueFamilyIndex = FamilyIndex };
            this.CommandPool = LogicalDevice.CreateCommandPool(createPoolInfo);
            AllocateBuffers(commandBufferCount);
        }

        internal override void Rebuild()
        {
            throw new NotImplementedException();
        }
    }
}
