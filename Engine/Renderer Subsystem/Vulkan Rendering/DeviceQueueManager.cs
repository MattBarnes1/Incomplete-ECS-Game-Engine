using Engine.Renderer.Vulkan;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Vulkan;
using Vulkan.Interop;
using Interop = Vulkan;

namespace EngineRenderer.Vulkan
{
    public class DeviceQueueManager
    {        
        internal class QueueManagerItem
        {
            internal LinkedList<SharedQueueItem> myList;
            
            public QueueManagerItem()
            {
                this.myList = new LinkedList<SharedQueueItem>();
            }

            internal void AddQueue(SharedQueueItem myNewQueueItem)
            {
                myList.AddLast(myNewQueueItem);
            }

            internal Engine.Renderer.Vulkan.Queue CreateQueue()
            {

                SharedQueueItem EndItem = myList.First.Value;
                Engine.Renderer.Vulkan.Queue myQueue = null;
                while(myQueue == null)
                {
                    var firstItem = myList.First.Value;
                    if (!firstItem.IsFull())
                    {
                        myQueue = myList.First.Value.CreateQueue();
                    } 
                    else
                    {
                        myList.RemoveFirst();
                        myList.AddLast(firstItem);
                        if(myList.First.Value.Equals(EndItem))
                        {
                            break;
                        }
                    }
                }

                return myQueue;
            }

            internal int GetRemainingQueueCount()
            {
               return myList.First.Value.GetRemainingQueueCount();
            }
        }
        /// <summary>
        /// When a shared queue is used it is decreased across the board so that each can be used only once.
        /// </summary>
        [TestReminder("Shared Queue", "Adding and removing queues")]
        internal struct SharedQueueItem
        {
            internal uint QueueFamilyIndex;
            public int[] QueueCountRemaining { get; private set; }
            internal VulkanLogicalDevice myDevice;
            private Extent3D minImageTransferGranularity;
            private uint timestampValidBits;


            public SharedQueueItem(uint queueFamilyIndex, uint queueCount, VulkanLogicalDevice vulkanLogicalDevice, Extent3D minImageTransferGranularity, uint timestampValidBits) : this()
            {
                this.QueueFamilyIndex = queueFamilyIndex;
                this.QueueCountRemaining = new int[queueCount];
                for(int i = 0; i < queueCount; i++)
                {
                    QueueCountRemaining[i] = i;
                }
                this.myDevice = vulkanLogicalDevice;
                this.minImageTransferGranularity = minImageTransferGranularity;
                this.timestampValidBits = timestampValidBits;
            }

            internal Engine.Renderer.Vulkan.Queue CreateQueue()
            {
                uint Next = GetNext();
                uint Value = (uint)QueueCountRemaining[Next];
                QueueCountRemaining[Next] = -1;
                return new Engine.Renderer.Vulkan.Queue(myDevice, QueueFamilyIndex, Value);
            }

            public uint GetNext()
            {
                for (uint i = 0; i < QueueCountRemaining.Length; i++)
                {
                    if (QueueCountRemaining[i] != -1)
                    {
                        return i;
                    }
                }
                throw new Exception("Invalid use of the Queue Creation!");
            }

            internal bool IsFull()
            {
                for(int i = 0; i < QueueCountRemaining.Length; i++)
                {
                    if(QueueCountRemaining[i] != -1)
                    {
                        return false;
                    }
                }
                return true;
            }

            internal int GetRemainingQueueCount()
            {
                int Remaining = QueueCountRemaining.Length;
                for (int i = 0; i < QueueCountRemaining.Length; i++)
                {
                    if(QueueCountRemaining[i] == -1)
                    {
                        Remaining--;
                    }
                }
                return Remaining;
            }
        }

        internal int GetFreeQueueCount(QueueFlags graphics)
        {
            return myQueueLookup[graphics].GetRemainingQueueCount();
        }

        //TODO: eventually expand this 
        public DeviceQueueManager(VulkanPhysicalDevice selectedPhysicalDevice, VulkanLogicalDevice vulkanLogicalDevice)
        {
            var Properties = selectedPhysicalDevice.QueueFamilyProperties;
            if (Properties == null) throw new Exception("Unable to find queue family properties!");


           // myDeviceCreateQueues = new Interop.DeviceQueueCreateInfo[Properties.Length];
#if DEBUG
            Debug.WriteLine("Expected number of Queues to Create: " + Properties.Length);
#endif
            for(int index =0; index < Properties.Length; index++)
            {
                var A = Properties[index];
                QueueManagerItem myValue;
                SharedQueueItem myNewQueueItem = new SharedQueueItem((uint)index, A.QueueCount, vulkanLogicalDevice, A.MinImageTransferGranularity, A.TimestampValidBits);
                foreach (Interop.QueueFlags G in Enum.GetValues(typeof(Interop.QueueFlags)))
                {
                    if ((A.QueueFlags & G) == G)
                    {
                       //TODO: one main graphics queue restriction?
                        {

                        }
                        if (!myQueueLookup.TryGetValue(G, out myValue))
                        {
                            myValue = new QueueManagerItem();
                            myQueueLookup[G] = myValue;
                        }
                        myValue.AddQueue(myNewQueueItem);
                    }
                }
            }


        }

        internal Interop.DeviceQueueCreateInfo[] GetDeviceQueueCreateArray()
        {
            Interop.DeviceQueueCreateInfo[] myInformation;
            HashSet<SharedQueueItem> myQueuedItems = new HashSet<SharedQueueItem>();
            foreach(var A in myQueueLookup)
            {
                foreach(var B in A.Value.myList)
                {
                    if (myQueuedItems.Contains(B)) continue;
                    myQueuedItems.Add(B);
                }
            }
            myInformation = new Interop.DeviceQueueCreateInfo[myQueuedItems.Count];
            float Priority = 1f;
            int DQCIIndex = 0;
            foreach(SharedQueueItem A in myQueuedItems)
            {
                myInformation[DQCIIndex] = new Interop.DeviceQueueCreateInfo();
                myInformation[DQCIIndex].QueueCount = (uint)A.QueueCountRemaining.Length;
                myInformation[DQCIIndex].QueueFamilyIndex = A.QueueFamilyIndex;
                myInformation[DQCIIndex].QueuePriorities = new float[A.QueueCountRemaining.Length];
                for(int i = 0; i < A.QueueCountRemaining.Length; i++)
                {
                    myInformation[DQCIIndex].QueuePriorities[i] = Priority;
                }
                Priority -= 0.1f;
                DQCIIndex++;
            }


            return myInformation;
        }

        Dictionary<Interop.QueueFlags, QueueManagerItem> myQueueLookup = new Dictionary<Interop.QueueFlags, QueueManagerItem>();

        internal Engine.Renderer.Vulkan.Queue GetQueue(Interop.QueueFlags byFlag)
        {
            return myQueueLookup[byFlag].CreateQueue();
        }
    }
}