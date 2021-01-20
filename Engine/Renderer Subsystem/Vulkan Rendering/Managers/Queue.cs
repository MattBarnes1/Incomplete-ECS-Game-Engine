using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;
using Interoperability = Vulkan; 

namespace Engine.Renderer.Vulkan
{
    public class Queue : Interoperability.Queue
    {
        public Semaphore myQueueSemaphore { get; }
        public uint QueueFamilyIndex { get; }
        public uint QueueIndex { get; }
        public Queue(Device myDevice, uint QueueFamilyIndex, uint QueueIndex)
        {
            SemaphoreCreateInfo myInfo = new SemaphoreCreateInfo();
            myQueueSemaphore = myDevice.CreateSemaphore(myInfo);
            this.QueueFamilyIndex = QueueFamilyIndex;
            this.QueueIndex = QueueIndex;
            myDevice.GetQueue(this, QueueFamilyIndex, QueueIndex);
        }

        public void SubmitQueue()
        {

        }
    }
}
