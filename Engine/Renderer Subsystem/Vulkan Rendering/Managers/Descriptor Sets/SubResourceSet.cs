using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;
using EngineRenderer;
using System;
using System.Collections.Generic;
using Vulkan;

namespace Engine.Renderer.Vulkan.Descriptor_Sets
{
    /// <summary>
    /// Every shader holds a SubResourceSet that is later combined to create the resource set. 
    /// </summary>
    public class SubResourceSet
    {
        public SubResourceSet(ShaderStageFlags myStage)
        {
            this.ShaderStage = myStage;
            myDescriptorTypeCount = new uint[Enum.GetValues(typeof(DescriptorType)).Length];
        }

        DescriptorSet myDescriptorSet;
        ~SubResourceSet()
        {
            //VulkanRenderer.myDescriptorPoolManager.Deallocate(this);
        }

        public VulkanPhysicalDevice Device { get; }

        public DescriptorSet GetDescriptorSet()
        {
            return myDescriptorSet;
        }

       

        internal Dictionary<String, DescriptorInformation> myIDsToItems = new Dictionary<string, DescriptorInformation>();

        public ShaderStageFlags ShaderStage { get; }

        public uint[] myDescriptorTypeCount { get; }
        public uint GetRequestedDescriptorSetSize(DescriptorType descriptorType)
        {
            return myDescriptorTypeCount[(uint)descriptorType];
        }



    /*internal DescriptorSetLayout GetDescriptorLayout()
        {
            List<DescriptorSetLayoutBinding> myBindings = new List<DescriptorSetLayoutBinding>();
            foreach (var A in myIDsToItems)
            {
                var Item = new DescriptorSetLayoutBinding();
                Item.DescriptorType = (DescriptorType)A.Value.myType;
                Item.StageFlags = this.ShaderStage;
                Item.DescriptorCount = A.Value.DescriptorCount;
                Item.Binding = A.Value.Binding;
                myBindings.Add(Item);
            }
            var descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo
            {
                Bindings = myBindings.ToArray()
            };
            return new DescriptorSetLayout(VulkanRenderer.SelectedLogicalGraphicsDevice, descriptorSetLayoutCreateInfo);
        }*/

        public void AddDescriptor(string ReferenceName, DescriptorType uniformBuffer, uint Binding, uint DescriptorCount = 1)
        {
            if (myIDsToItems.ContainsKey(ReferenceName)) throw new Exception("Attempted to add two descriptors that had the same name.");
            foreach(var A in myIDsToItems)
            {
                if(A.Value.Binding == Binding) throw new Exception("Attempted to add two descriptors that have the same bindings.");
            }
            
            myIDsToItems[ReferenceName] = new DescriptorInformation(Binding, uniformBuffer, this.ShaderStage, DescriptorCount);
            myDescriptorTypeCount[(uint)uniformBuffer] += 1;
        }

        internal SubResourceSet Copy()
        {
            SubResourceSet mySet = new SubResourceSet(this.ShaderStage);
            foreach(var A in myIDsToItems)
            {
                mySet.myIDsToItems.Add(A.Key, A.Value);
                mySet.myDescriptorTypeCount[(uint)A.Value.myType] += 1;
            }
            return mySet;
        }

        internal void SetDescriptorSet(DescriptorSet descriptorSet)
        {
            myDescriptorSet = descriptorSet;
        }

        internal void MergeFrom(SubResourceSet value)
        {
            foreach(var B in value.myIDsToItems)
            {
                AddDescriptor(B.Key, B.Value.myType, B.Value.Binding, B.Value.DescriptorCount);
            }
        }
    }
}
