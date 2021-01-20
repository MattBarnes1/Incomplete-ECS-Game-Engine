﻿/* Please note that this file is generated by the VulkanSharp's generator. Do not edit directly.

   Licensed under the MIT license.

   Copyright 2016 Xamarin Inc

   This notice may not be removed from any source distribution.
   See LICENSE file for licensing details.
*/

using EngineRenderer;
using System;
using System.Runtime.InteropServices;

namespace Vulkan
{
    public class VulkanBuffer : INonDispatchableHandleMarshalling, IDisposable
    {
        private AllocationCallbacks Allocator;
        VulkanPhysicalDevice myActiveDevice;


        /* public Vulkan.Buffer CreateUniformBuffer()
         {
             var uniformBufferData = new AreaUniformBuffer
             {
                 width = SurfaceCapabilities.CurrentExtent.Width,
                 height = SurfaceCapabilities.CurrentExtent.Height
             };

             return CreateBuffer(uniformBufferData, BufferUsageFlags.UniformBuffer, typeof(AreaUniformBuffer));
         }
         private Vulkan.Buffer CreateBuffer( object values, BufferUsageFlags usageFlags, System.Type type)
         {
             var array = values as System.Array;
             var length = (array != null) ? array.Length : 1;
             var size = System.Runtime.InteropServices.Marshal.SizeOf(type) * length;
             var createBufferInfo = new BufferCreateInfo
             {
                 Size = size,                
                 Usage = usageFlags,
                 SharingMode = SharingMode.Exclusive,
                 QueueFamilyIndices = new uint[] { 0 }
             };
             return CreateBufferBufferCreateInfoCanBeSet(values, type, length, size, createBufferInfo);
         }

         private Vulkan.Buffer CreateBufferBufferCreateInfoCanBeSet(object values, Type type, int length, int size, BufferCreateInfo createBufferInfo)
         {
             var buffer = LogicalDevice.CreateBuffer(createBufferInfo);
             var memoryReq = LogicalDevice.GetBufferMemoryRequirements(buffer);
             var allocInfo = new MemoryAllocateInfo { AllocationSize = memoryReq.Size };
             var memoryProperties = myPhysicalDevice.GetMemoryProperties();
             bool heapIndexSet = false;
             var memoryTypes = memoryProperties.MemoryTypes;

             for (uint i = 0; i < memoryProperties.MemoryTypeCount; i++)
             {
                 if (((memoryReq.MemoryTypeBits >> (int)i) & 1) == 1 &&
                     (memoryTypes[i].PropertyFlags & MemoryPropertyFlags.HostVisible) == MemoryPropertyFlags.HostVisible)
                 {
                     allocInfo.MemoryTypeIndex = i;
                     heapIndexSet = true;
                 }
             }

             if (!heapIndexSet)
                 allocInfo.MemoryTypeIndex = memoryProperties.MemoryTypes[0].HeapIndex;

             var deviceMemory = LogicalDevice.AllocateMemory(allocInfo);
             var memPtr = LogicalDevice.MapMemory(deviceMemory, 0, size, 0);

             if (type == typeof(float))
                 System.Runtime.InteropServices.Marshal.Copy(values as float[], 0, memPtr, length);
             else if (type == typeof(short))
                 System.Runtime.InteropServices.Marshal.Copy(values as short[], 0, memPtr, length);
             else if (type == typeof(AreaUniformBuffer))
                 System.Runtime.InteropServices.Marshal.StructureToPtr(values, memPtr, false);

             LogicalDevice.UnmapMemory(deviceMemory);
             LogicalDevice.BindBufferMemory(buffer, deviceMemory, 0);

             return buffer;
         }
         VertextBuffer VertBuffer = 
     */


        public unsafe VulkanBuffer(VulkanPhysicalDevice myDevice, BufferCreateInfo createBufferInfo, AllocationCallbacks pAllocator = null) //Tested
        {
            if (myDevice == null) throw new VULKANDEVICE_NO_ACTIVE_DEVICE();
            this.Allocator = pAllocator;
            this.myActiveDevice = myDevice;
            CreateBuffer(myDevice, createBufferInfo, pAllocator);
        }
        protected unsafe VulkanBuffer() //Tested
        {

        }
        DeviceMemory deviceMemory;
        public unsafe void CopyFromMemory<T>(T myManagedArray) where T : struct
        {
            Count = 1;
            int SizeOfT = Marshal.SizeOf(typeof(T));
            IntPtr memPtr = SetupCopy<T>(SizeOfT);
            System.Runtime.InteropServices.Marshal.StructureToPtr(myManagedArray, memPtr, false);
            EndCopy();
        }

        public unsafe void CopyFromMemory<T>(T[] myManagedArray) where T : unmanaged
        {
            int SizeOfT = sizeof(T);
            Count = (uint)myManagedArray.Length;
            int TotalSize = myManagedArray.Length * SizeOfT;
            IntPtr memPtr = SetupCopy<T>(TotalSize);
            int longCastCountRemainder = TotalSize % sizeof(long);
            int Iterator = ((TotalSize - longCastCountRemainder) / sizeof(long));
            if (typeof(T) == typeof(float))
                System.Runtime.InteropServices.Marshal.Copy(myManagedArray as float[], 0, memPtr, myManagedArray.Length);
            else if (typeof(T) == typeof(short))
                System.Runtime.InteropServices.Marshal.Copy(myManagedArray as short[], 0, memPtr, myManagedArray.Length);
            else if (typeof(T) == typeof(uint))
            {
                System.Runtime.InteropServices.Marshal.Copy(myManagedArray as int[], 0, memPtr, myManagedArray.Length);
            }
            else
            {
                throw new Exception("Invalid Type Passed to Buffer");
            }

            EndCopy();
        }

        public uint Count { get; private set;}

        private unsafe void EndCopy()
        {
            myActiveDevice.LogicalDevice.UnmapMemory(deviceMemory);
            myActiveDevice.LogicalDevice.BindBufferMemory(this, deviceMemory, 0);
        }

        public DeviceSize SizeOfBuffer { get; private set; }
        public DeviceSize SizeOfInternalStructure;
        MemoryAllocateInfo allocInfo;
        private unsafe IntPtr SetupCopy<T>(int mySize)
        {
            if(SizeOfBuffer == 0)
            {
                SizeOfInternalStructure = mySize;
                var memoryReq = myActiveDevice.LogicalDevice.GetBufferMemoryRequirements(this);
                SizeOfBuffer = memoryReq.Size;
                allocInfo = new MemoryAllocateInfo { AllocationSize = memoryReq.Size };
                var memoryProperties = myActiveDevice.GetMemoryProperties();
                bool heapIndexSet = false;
                var memoryTypes = memoryProperties.MemoryTypes;

                for (uint i = 0; i < memoryProperties.MemoryTypeCount; i++)
                {
                    if (((memoryReq.MemoryTypeBits >> (int)i) & 1) == 1 &&
                        (memoryTypes[i].PropertyFlags & MemoryPropertyFlags.HostVisible) == MemoryPropertyFlags.HostVisible)
                    {
                        allocInfo.MemoryTypeIndex = i;
                        heapIndexSet = true;
                    }
                }
                if (!heapIndexSet)
                    allocInfo.MemoryTypeIndex = memoryProperties.MemoryTypes[0].HeapIndex;
            }

            deviceMemory = new DeviceMemory(myActiveDevice.LogicalDevice, allocInfo);
            return myActiveDevice.LogicalDevice.MapMemory(deviceMemory, 0, mySize, 0);
        }


        protected unsafe void CreateBuffer(VulkanPhysicalDevice myDevice, BufferCreateInfo createBufferInfo, AllocationCallbacks pAllocator = null)
        {
            if (myDevice == null) 
                throw new VULKANDEVICE_NO_ACTIVE_DEVICE();
            this.Allocator = pAllocator;
            this.myActiveDevice = myDevice;

            SizeOfInternalStructure = createBufferInfo.Size;
            Result result;
            fixed (UInt64* ptrpBuffer = &this.m)
            {
                result = Interop.NativeMethods.vkCreateBuffer(myDevice.LogicalDevice.m, createBufferInfo != null ? createBufferInfo.m : (Interop.BufferCreateInfo*)default(IntPtr), pAllocator != null ? pAllocator.m : null, ptrpBuffer);
            }
            if (result != Result.Success)
                throw new ResultException(result);
        }

        protected unsafe VulkanBuffer(BufferCreateInfo createBufferInfo, AllocationCallbacks pAllocator) : this(VulkanRenderer.SelectedPhysicalDevice, createBufferInfo, pAllocator)
        {

        }



		internal UInt64 m;

		UInt64 INonDispatchableHandleMarshalling.Handle {
			get {
				return m;
			}
		}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                deviceMemory?.Dispose();
                unsafe
                {
                    fixed (UInt64* ptrpBuffer = &this.m)
                    {
                        Interop.NativeMethods.vkDestroyBuffer(this.myActiveDevice.LogicalDevice.m, this.m, null);
                    }
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

         ~VulkanBuffer()
         {
           // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
         }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
             GC.SuppressFinalize(this);
        }

        public DeviceSize GetSize()
        {
            return SizeOfBuffer;
        }
        #endregion
    }
}