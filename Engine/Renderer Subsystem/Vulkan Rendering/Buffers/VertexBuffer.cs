
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer.Vulkan
{
   /* public class VertexBuffer : VulkanBuffer
    {
        public VertexBuffer(int Size, VulkanDevice myDevice, uint[] QueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
       : base(myDevice, new BufferCreateInfo { Size = Size, Usage = BufferUsageFlags.VertexBuffer, SharingMode = myMode, QueueFamilyIndices = QueueFamilyIndices }, pAllocator)
        {

        }
        public VertexBuffer(int Size, uint[] QueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
: base(VulkanDevice.myActiveDevice, new BufferCreateInfo { Size = Size, Usage = BufferUsageFlags.VertexBuffer, SharingMode = myMode, QueueFamilyIndices = QueueFamilyIndices }, pAllocator)
        {

        }

        protected VertexBuffer() { }

    }*/

    public class VertexBuffer<T> : VulkanBuffer, IDisposable where T:unmanaged
    {
        public unsafe VertexBuffer(T[] myManagedArray, VulkanPhysicalDevice myDevice, uint[] QueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length*mySize, Usage = BufferUsageFlags.VertexBuffer, SharingMode = myMode, QueueFamilyIndices = QueueFamilyIndices };
            CreateBuffer(myDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }
        public unsafe VertexBuffer(T[] myManagedArray, uint[] QueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length * mySize, Usage = BufferUsageFlags.VertexBuffer, SharingMode = myMode, QueueFamilyIndices = QueueFamilyIndices };
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }

        public unsafe VertexBuffer(T[] myManagedArray, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            var Device = VulkanRenderer.SelectedPhysicalDevice;
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length * mySize, Usage = BufferUsageFlags.VertexBuffer, SharingMode = myMode, QueueFamilyIndices = new uint[] { VulkanRenderer.ActiveGraphicsFamilyQueueIndex } };
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }

        
    }
}
