using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Renderer.Vulkan
{
    public enum VertexBuilderFormat : uint
    {
        FLOAT1 = 1 * sizeof(float),
        FLOAT2 = 2 * sizeof(float),
        FLOAT3 = 3 * sizeof(float),
        FLOAT4 = 4 * sizeof(float),
    }
    [TestReminder("VertexBuilder", "Proper offsets are set for each type")]
    public class VertexBuilder
    {
        public VertexBuilder(uint Binding = 0)
        {
            this.myBinding = Binding;
            myBindingVertex.Binding = Binding;

        }



        List<VertexInputAttributeDescription> myVertexAttributes = new List<VertexInputAttributeDescription>();
        VertexInputBindingDescription myBindingVertex = new VertexInputBindingDescription { InputRate = VertexInputRate.Vertex };
        private readonly uint myBinding;
        private uint LastLocation = 0;

        internal PipelineVertexInputStateCreateInfo GetStateCreateInfo()
        {            //https://github.com/SaschaWillems/Vulkan/blob/master/examples/subpasses/subpasses.cpp
            if(myVertexAttributes.Count == 0)
            {
                return new PipelineVertexInputStateCreateInfo();
            } 
            else
            {
                return new PipelineVertexInputStateCreateInfo
                {
                    VertexBindingDescriptions = new VertexInputBindingDescription[] { myBindingVertex },
                    VertexAttributeDescriptions = myVertexAttributes.ToArray()
                };
            }
        }

        internal PipelineVertexInputStateCreateInfo GetDefaultStateCreateInfo()
        {

            /*A vertex binding describes at which rate to load data from memory throughout the vertices. 
             * It specifies the number of bytes between data entries and whether to move to the next data entry after each vertex or after each instance.
             * All of our per-vertex data is packed together in one array, so we're only going to have one binding. 
             * The binding parameter specifies the index of the binding in the array of bindings. 
             * The stride parameter specifies the number of bytes from one entry to the next, and the inputRate parameter can have one of the following values:*/
           /* myVertexDescriptions.Add(new VertexInputBindingDescription { Binding = 0, InputRate = VertexInputRate.Vertex, Stride = 14 * sizeof(float) });


            /*The binding parameter tells Vulkan from which binding the per-vertex data comes. 
             * The location parameter references the location directive of the input in the vertex shader. 
             * The input in the vertex shader with location 0 is the position, which has two 32-bit float components.*/
           /* myVertexAttributes.Add(new VertexInputAttributeDescription { Format = Format.R32G32B32Sfloat, Binding = 0, Location = 0, Offset = 0}); //position
            myVertexAttributes.Add(new VertexInputAttributeDescription { Format = Format.R32G32B32Sfloat, Binding = 0, Location = 1, Offset = 3 * sizeof(float) }); //color
            myVertexAttributes.Add(new VertexInputAttributeDescription { Format = Format.R32G32B32Sfloat, Binding = 0, Location = 2, Offset = 6 * sizeof(float) }); //normal
            myVertexAttributes.Add(new VertexInputAttributeDescription { Format = Format.R32G32B32Sfloat, Binding = 0, Location = 3, Offset = 9 * sizeof(float) }); //tangent
            */
            AddVertexAttribute(VertexBuilderFormat.FLOAT3);
            AddVertexAttribute(VertexBuilderFormat.FLOAT2);
            AddVertexAttribute(VertexBuilderFormat.FLOAT3);
            AddVertexAttribute(VertexBuilderFormat.FLOAT3);
            AddVertexAttribute(VertexBuilderFormat.FLOAT3);
            return GetStateCreateInfo();



            /*  struct Vertex
              {
                  float pos[3];
                  float uv[2];
                  float col[3];
                  float normal[3];
                  float tangent[3];
              };*/
        }

        public PipelineVertexInputStateCreateInfo GetPipelineData()
        {
            return new PipelineVertexInputStateCreateInfo
            {
                VertexBindingDescriptions = new VertexInputBindingDescription[] { myBindingVertex },
                VertexAttributeDescriptions = myVertexAttributes.ToArray()
            };
        }

        public void AddVertexAttribute(VertexBuilderFormat myFormat)
        {
            uint offset = 0;
            VertexInputAttributeDescription myLastItem;
            if (myVertexAttributes.Count != 0)
            {
                myLastItem = myVertexAttributes.Last();
                offset = myLastItem.Offset + GetOffsetSizeFromVulkanFormat(myLastItem.Format);
            }
            Format VulkanFormat = ConvertVertexFormatToVulkan(myFormat);
            myVertexAttributes.Add(new VertexInputAttributeDescription { Format = VulkanFormat, Binding = myBinding, Location = LastLocation, Offset = offset });
            LastLocation++;
            myLastItem = myVertexAttributes.Last();
            myBindingVertex.Stride = myLastItem.Offset + GetOffsetSizeFromVulkanFormat(myLastItem.Format);

        }



        private Format ConvertVertexFormatToVulkan(VertexBuilderFormat myFormat)
        {
            switch (myFormat)
            {
                case VertexBuilderFormat.FLOAT1:
                    return Format.R32Sfloat;
                case VertexBuilderFormat.FLOAT2:
                    return Format.R32G32Sfloat;
                case VertexBuilderFormat.FLOAT3:
                    return Format.R32G32B32Sfloat;
                case VertexBuilderFormat.FLOAT4:
                    return Format.R32G32B32A32Sfloat;
            }
            throw new Exception("Invalid VertexBuilder Format passed to GetOffsetSizeFromVulkanFormat!");
        }

        private uint GetOffsetSizeFromVulkanFormat(Format myFormat)
        {
            switch(myFormat)
            {
                case Format.R32G32B32A32Sfloat:
                    return 4 * sizeof(float);
                case Format.R32G32B32Sfloat:
                    return 3 * sizeof(float);
                case Format.R32G32Sfloat:
                    return 2 * sizeof(float);
                case Format.R32Sfloat:
                    return 2 * sizeof(float);
            }
            throw new Exception("Invalid Format passed to GetOffsetSizeFromVulkanFormat!");
        }





        //https://vulkan-tutorial.com/Vertex_buffers/Vertex_input_description
        /*Just like fragColor, the layout(location = x) annotations assign indices to the inputs that we can later use to reference them. 
         * It is important to know that some types, like dvec3 64 bit vectors, use multiple slots. That means that the index after it must be at least 2 higher:*/

        /*vertexInputBindingDescription = new VertexInputBindingDescription
            {

                Stride = 3 * sizeof(float),
                InputRate = VertexInputRate.Vertex
            };
            vertexInputAttributeDescription = new VertexInputAttributeDescription
            {
                Format = Format.R32G32B32Sfloat
            };
            vertexInputStateCreateInfo = new PipelineVertexInputStateCreateInfo
            {
                VertexBindingDescriptions = new VertexInputBindingDescription[] { vertexInputBindingDescription },
                VertexAttributeDescriptions = new VertexInputAttributeDescription[] { vertexInputAttributeDescription }
            };*/



    }
}
