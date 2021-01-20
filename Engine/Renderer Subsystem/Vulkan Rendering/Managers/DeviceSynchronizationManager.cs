using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Vulkan.Managers
{
    public class DeviceSynchronizationManager
    {
        public FenceCreateInfo fenceInfo;
        public Fence fence { get; private set; }
        public SemaphoreCreateInfo semaphoreInfo { get; private set; }

        public Semaphore DrawSemaphore { get; private set; }
        internal void Initialize()
        {

        }
    }
}
