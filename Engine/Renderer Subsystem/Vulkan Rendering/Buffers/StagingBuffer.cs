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
    public class StagingBuffer<T> : VulkanBuffer where T:unmanaged
    {
        public unsafe StagingBuffer(T[] myManagedArray, VulkanPhysicalDevice myDevice, uint[] VisibleToQueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length * mySize, Usage = BufferUsageFlags.TransferSrc, SharingMode = myMode, QueueFamilyIndices = VisibleToQueueFamilyIndices };
            CreateBuffer(myDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }
        public unsafe StagingBuffer(T[] myManagedArray, uint[] VisibleToQueueFamilyIndices, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length * mySize, Usage = BufferUsageFlags.TransferSrc, SharingMode = myMode, QueueFamilyIndices = VisibleToQueueFamilyIndices };
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }

        public unsafe StagingBuffer(T[] myManagedArray, SharingMode myMode = SharingMode.Exclusive, AllocationCallbacks pAllocator = null)
        {
            int mySize = Marshal.SizeOf(typeof(T));
            var Device = VulkanRenderer.SelectedPhysicalDevice;
            BufferCreateInfo myBuffer = new BufferCreateInfo { Size = myManagedArray.Length * mySize, Usage = BufferUsageFlags.TransferSrc, SharingMode = myMode, QueueFamilyIndices = new uint[] { VulkanRenderer.ActiveGraphicsFamilyQueueIndex } };
            CreateBuffer(VulkanRenderer.SelectedPhysicalDevice, myBuffer, pAllocator);
            CopyFromMemory<T>(myManagedArray);
        }


    }
}
