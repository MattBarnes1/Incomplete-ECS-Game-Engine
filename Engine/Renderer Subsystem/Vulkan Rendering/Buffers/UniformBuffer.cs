using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Low_Level.Vulkan.Classes_By_Me.Buffer
{
    public class UniformBuffer<T> : VulkanBuffer where T:struct
    {

        public unsafe UniformBuffer(T myManagedArray, VulkanPhysicalDevice myDevice, uint[] QueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size =  mySize, Usage = BufferUsageFlags.UniformBuffer, SharingMode = myMode, QueueFamilyIndices = QueueFamilyIndices };
            CreateBuffer(myDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }
        public unsafe UniformBuffer(T myManagedArray, uint[] QueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size =  mySize, Usage = BufferUsageFlags.UniformBuffer, SharingMode = myMode, QueueFamilyIndices = QueueFamilyIndices };
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }



        public unsafe UniformBuffer(T myManagedArray, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size =  mySize, Usage = BufferUsageFlags.UniformBuffer, SharingMode = myMode, QueueFamilyIndices = new uint[] { VulkanRenderer.ActiveGraphicsFamilyQueueIndex } };
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }

        public UniformBuffer()
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = mySize, Usage = BufferUsageFlags.UniformBuffer, SharingMode = SharingMode.Exclusive, QueueFamilyIndices = new uint[] { VulkanRenderer.ActiveGraphicsFamilyQueueIndex } }; //TODO: Fix this in the buffers -_-
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, null);
        }
    }
}
