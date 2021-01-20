using Engine.Renderer.Vulkan.Descriptor_Sets;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer
{
    public class DescriptorSetPoolManager
    {
        Device myDevice;
        List<DescriptorPoolObject> myActivePool = new List<DescriptorPoolObject>();
        public DescriptorSetPoolManager(Device myDevice)
        {
            this.myDevice = myDevice;
            myActivePool.Add(new DescriptorPoolObject(this.myDevice));
        }

        readonly object _lock = new object();

        internal void Deallocate(ResourceSet resourceSet)
        {
            throw new NotImplementedException();
        }

        public bool Allocate(ResourceSet myResources)
        {
            lock (_lock)
            {
                foreach (DescriptorPoolObject poolInfo in myActivePool)
                {
                    if (poolInfo.TryAllocate(myResources))
                    {
                        return true;
                    }
                }
                var newPool = new DescriptorPoolObject(myDevice);
                return newPool.TryAllocate(myResources);
            }
        }

        private class DescriptorPoolObject
        {
            DescriptorPool myNewPoolObject;
            uint[] DescriptorSetTypesRemainingInPool;
            uint TotalSetsRemaining = 1000;
            DescriptorType[] myEnumValues;
            public DescriptorPoolObject(Device myDevice)
            {
                myEnumValues = (DescriptorType[])Enum.GetValues(typeof(DescriptorType));
                int PoolItems = myEnumValues.Count();
                DescriptorPoolSize[] myPoolSizeValues = new DescriptorPoolSize[PoolItems];
                DescriptorSetTypesRemainingInPool = new uint[PoolItems];
                for (int i = 0; i < myEnumValues.Length; i++)
                {
                    myPoolSizeValues[i].Type = (DescriptorType)myEnumValues[i];
                    myPoolSizeValues[i].DescriptorCount = 100;
                    DescriptorSetTypesRemainingInPool[i] = 100;
                }
                DescriptorPoolCreateInfo myCreateInformation = new DescriptorPoolCreateInfo();
                //TODO: Deallocate
                myCreateInformation.MaxSets = TotalSetsRemaining;
                myCreateInformation.PoolSizes = myPoolSizeValues;
                myNewPoolObject = myDevice.CreateDescriptorPool(myCreateInformation);
            }

            public unsafe bool TryAllocate(ResourceSet myResources)
            {

                myEnumValues = (DescriptorType[])Enum.GetValues(typeof(DescriptorType));
                int PoolItems = myEnumValues.Count();
                DescriptorPoolSize[] myPoolSizeValues = new DescriptorPoolSize[PoolItems];
                uint* myValues = stackalloc uint[PoolItems];
                if (TotalSetsRemaining > 0)
                {
                    for (int i = 0; i < myEnumValues.Length; i++)
                    {
                        myValues[i] = myResources.GetRequestedDescriptorSetSize(myEnumValues[i]);
                        if(DescriptorSetTypesRemainingInPool[i] < myValues[i])
                        {
                            return false;
                        }
                    }
                }
                for(int i = 0; i < PoolItems; i++)
                {
                    DescriptorSetTypesRemainingInPool[i] -= myValues[i];
                }
                myResources.AllocateFrom(myNewPoolObject);
                return true;
            }
        }


    }
}
