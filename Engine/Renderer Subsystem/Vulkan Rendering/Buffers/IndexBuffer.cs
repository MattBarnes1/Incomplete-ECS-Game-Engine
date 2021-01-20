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
    public class IndexBuffer<T> : VulkanBuffer, IDisposable where T:unmanaged
    {
        public unsafe IndexBuffer(T[] myManagedArray, VulkanPhysicalDevice myDevice, uint[] QueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length * mySize, Usage = BufferUsageFlags.IndexBuffer, SharingMode = myMode, QueueFamilyIndices = QueueFamilyIndices };
            CreateBuffer(myDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }
        public unsafe IndexBuffer(T[] myManagedArray, uint[] QueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length * mySize, Usage = BufferUsageFlags.IndexBuffer, SharingMode = myMode, QueueFamilyIndices = QueueFamilyIndices };
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }

        public unsafe IndexBuffer(T[] myManagedArray, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            var Device = VulkanRenderer.SelectedPhysicalDevice;
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length * mySize, Usage = BufferUsageFlags.IndexBuffer, SharingMode = myMode, QueueFamilyIndices = new uint[] { VulkanRenderer.ActiveGraphicsFamilyQueueIndex } };
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }




    }
}
