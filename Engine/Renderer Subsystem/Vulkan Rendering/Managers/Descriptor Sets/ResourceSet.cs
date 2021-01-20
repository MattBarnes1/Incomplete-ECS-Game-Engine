using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer.Vulkan.Descriptor_Sets
{
    /// <summary>
    /// ResourceSet holds the information for allocating and deallocating the Descriptor Sets from Vulkan API. Controls the subresources and allocation/garbage collection of resources.
    /// </summary>
    [TestReminder("Resource Set")]
    public partial class ResourceSet
    {
        uint[] myDescriptorTypeCount = new uint[Enum.GetValues(typeof(DescriptorType)).Length];
        Dictionary<String, DescriptorInformation> myIDsToItems = new Dictionary<string, DescriptorInformation>();
        Dictionary<uint, DescriptorInformation> myBindingNumberToItems = new Dictionary<uint, DescriptorInformation>();
        internal DescriptorSet[] myDSet { get; set; }
        public ResourceSet()
        {
        }




        public ResourceSet(ResourceSet resourceSet)
        {
            Parallel.For(0, resourceSet.myDescriptorTypeCount.Count(), delegate (int i)
            {
                myDescriptorTypeCount[i] += resourceSet.myDescriptorTypeCount[i];
            });

            foreach (var B in resourceSet.myIDsToItems)
            {
                myIDsToItems[B.Key] = B.Value;
            }
            foreach (var C in resourceSet.myBindingNumberToItems)
            {
                myBindingNumberToItems[C.Key] = C.Value;
            }
            Allocate();
        }

        public void MergeFrom(SubResourceSet mySet)
        {
            Parallel.For(0, mySet.myDescriptorTypeCount.Count(), delegate (int i)
            {
                myDescriptorTypeCount[i] += mySet.myDescriptorTypeCount[i];
            });

            foreach (var B in mySet.myIDsToItems)
            {
                DescriptorInformation myDescriptor;
                if (myBindingNumberToItems.TryGetValue(B.Value.Binding, out myDescriptor)) //if we already have the same binding here...
                {
                    ValidateAndCombineDescriptorInformation(myDescriptor, B.Value); //Make sure they match.
                } 
                else
                {
                    myDescriptor = B.Value; //Otherwise add it in.
                    myBindingNumberToItems[B.Value.Binding] = B.Value;                    
                }
                               
                if (myIDsToItems.ContainsKey(B.Key))
                {
                    if(!myIDsToItems[B.Key].Equals(myDescriptor))
                    {
                        throw new Exception("Two descriptors share the same internal ID, but aren't the same reference or binding!");
                    }
                }
                else
                {
                    myIDsToItems[B.Key] = myDescriptor;
                }

            }
        }

        private void ValidateAndCombineDescriptorInformation(DescriptorInformation myDescriptor, DescriptorInformation value)
        {
            if(myDescriptor.ShaderStage != value.ShaderStage)
            {
                myDescriptor.ShaderStage |= value.ShaderStage;
            }
            if(myDescriptor.myType != value.myType)
            {
                throw new Exception("Descriptor Information mismatch detected!");
            }
            if (myDescriptor.DescriptorCount != value.DescriptorCount)
            {
                throw new Exception("Descriptor Information mismatch detected!");
            }
        }

    




        public void Allocate()
        {
            VulkanRenderer.myDescriptorPoolManager.Allocate(this);
        }


    /*        public void Write(string name, DescriptorImageInfo myInfo)
    internal void WriteUniform<T>(string v, T mYBuffer) where T : struct*/
        internal void AllocateFrom(DescriptorPool myNewPoolObject)
        {
            DescriptorSetLayout myBindings = GetDescriptorSetLayout();

            DescriptorSetLayoutCreateInfo myInfo = new DescriptorSetLayoutCreateInfo();
           
            DescriptorSetAllocateInfo mySetAlloc = new DescriptorSetAllocateInfo(); //TODO: Garbage and memory stuff
            mySetAlloc.SetLayouts = new DescriptorSetLayout[] { myBindings };
            mySetAlloc.DescriptorPool = myNewPoolObject;

            myDSet = VulkanRenderer.SelectedLogicalDevice.AllocateDescriptorSets(mySetAlloc);
           // Debug.Assert(myDSet.Count() == myBindings.Count);
        }

        public DescriptorSetLayout GetDescriptorSetLayout()
        {
            List<DescriptorSetLayoutBinding> myBindings = new List<DescriptorSetLayoutBinding>();
            foreach (var A in myBindingNumberToItems)
            {
                var Item = new DescriptorSetLayoutBinding();
                Item.DescriptorType = (DescriptorType)A.Value.myType;
                Item.StageFlags = A.Value.ShaderStage;
                Item.DescriptorCount = A.Value.DescriptorCount;
                Item.Binding = A.Value.Binding;
                myBindings.Add(Item);
            }
            var descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo
            {
                Bindings = myBindings.ToArray()
            };
            return new DescriptorSetLayout(VulkanRenderer.SelectedLogicalDevice, descriptorSetLayoutCreateInfo);
        }

        public uint GetRequestedDescriptorSetSize(DescriptorType descriptorType)
        {
            return myDescriptorTypeCount[(uint)descriptorType];
        }

        internal void Write<T>(string v, T mYBuffer) where T : struct
        {
            var Value = myIDsToItems[v];
            var MVPBuffer = new UniformBuffer<T>(mYBuffer);
            WriteDescriptorSet mySet = new WriteDescriptorSet();
            mySet.DescriptorType = DescriptorType.UniformBuffer;
            mySet.DescriptorCount = Value.DescriptorCount;
            mySet.DstBinding = Value.Binding;
            mySet.DstSet = myDSet[Value.Binding];
            unsafe
            {
                mySet.BufferInfo = new DescriptorBufferInfo[] { new DescriptorBufferInfo { Buffer = MVPBuffer, Offset = 0, Range = MVPBuffer.SizeOfBuffer } };
            }
            VulkanRenderer.SelectedLogicalDevice.UpdateDescriptorSet(mySet, null);
            MVPBuffer.Dispose();
            mySet.Dispose();
        }

        public void Write(string name, DescriptorImageInfo myInfo)
        {
            var Value = myIDsToItems[name];

            WriteDescriptorSet mySet = new WriteDescriptorSet();
            mySet.DescriptorType = DescriptorType.UniformBuffer;
            mySet.DescriptorCount = Value.DescriptorCount;
            mySet.DstBinding = Value.Binding;
            mySet.DstSet = myDSet[Value.Binding];
            unsafe
            {
                mySet.ImageInfo = new DescriptorImageInfo[] { myInfo };
            }
            VulkanRenderer.SelectedLogicalDevice.UpdateDescriptorSet(mySet, null);
            mySet.Dispose();
        }


    }
}
