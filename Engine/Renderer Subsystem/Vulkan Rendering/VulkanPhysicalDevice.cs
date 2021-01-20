using Engine.Low_Level.Vulkan;
using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;
using Engine.Low_Level.Vulkan.Classes_By_Me.Frame_Buffer_Pool;
using Engine.Renderer;
using Engine.Renderer.Vulkan;
using Engine.Vulkan.Managers;
using EngineRenderer.Exceptions.VULKAN;
using EngineRenderer.Vulkan;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vulkan;

namespace EngineRenderer
{  
    /// <summary>
    /// The VulkanDevice class is responsible for holding the information on an available logical device that is later filtered by the renderer.
    /// </summary>
    public class VulkanPhysicalDevice : PhysicalDevice
    {
        public string DeviceName { get; }
        public uint ID { get; }

        public PhysicalDeviceMemoryProperties MemoryProperties { get; private set; }
        public QueueFamilyProperties[] QueueFamilyProperties { get; private set; }
        public VulkanLogicalDevice LogicalDevice { get; internal set; }
    
        //public SurfaceFormatKHR SelectedSurfaceFormat { get; internal set; }
        public bool isInitialized { get; private set; }

        public Format DepthBufferFormat { get; private set; }

        Format[] depthFormats = {
                Format.D32SfloatS8Uint,
                Format.D32Sfloat,
                Format.D24UnormS8Uint,
                Format.D16UnormS8Uint,
                Format.D16Unorm
            };

        internal uint GetMemoryIndexFromProperty(uint memoryTypeBits, MemoryPropertyFlags deviceLocal)
        {
            var memoryProperties = this.GetMemoryProperties();//TODO: call once?
            bool heapIndexSet = false;

            var memoryTypes = memoryProperties.MemoryTypes;

            for (uint i = 0; i < memoryProperties.MemoryTypeCount; i++)
            {
                if (((memoryTypeBits >> (int)i) & 1) == 1 &&
                    (memoryTypes[i].PropertyFlags & deviceLocal) == deviceLocal)
                {
                    return i;
                }
            }
            return memoryProperties.MemoryTypes[0].HeapIndex;
        }

        public VulkanPhysicalDevice(IntPtr thisnew)
        {
            this.m = thisnew;
            MemoryProperties = GetMemoryProperties();
            QueueFamilyProperties = GetQueueFamilyProperties();

            DepthBufferFormat = GetBestDepthBufferFormat();
        }

        private Format GetBestDepthBufferFormat()
        {
            foreach(var A in depthFormats)
            {
                FormatProperties myProperties = GetFormatProperties(A);
                if((myProperties.OptimalTilingFeatures & FormatFeatureFlags.DepthStencilAttachment) == FormatFeatureFlags.DepthStencilAttachment)
                {
                    return A;
                }
            }
            throw new Exception("Could not find appropriate depth buffer!");
        }

        public QueueFamilyProperties getQueueFamilyPropertiesFromIndex(uint i)
        {
            if (i >= this.QueueFamilyProperties.Length) throw new Exception("Invalid index requested for Queue Family Properties!");
            return this.QueueFamilyProperties[i];
        }

        public int GetQueueFamilyIndexByType(QueueFlags G)
        {
            for(int i = 0; i < this.QueueFamilyProperties.Length; i++)
            {
                var A = this.QueueFamilyProperties[i];
                if ((A.QueueFlags & G) == G)
                {
                    return i;
                }
            }
            return -1;
        }




        public bool SupportsType(QueueFlags G)
        {
            foreach(QueueFamilyProperties A in this.QueueFamilyProperties)
            {
                if((A.QueueFlags & G) == G)
                {
                    return true;
                }
            }
            return false;
        }



    }
}