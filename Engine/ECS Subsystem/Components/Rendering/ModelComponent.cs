using Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Engine.Renderer.Vulkan;
using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;
using Engine.Exceptions;
using System.Numerics;
using Engine.DataTypes;
using Engine.Entities;
using Vulkan;
using Engine.Renderer.Vulkan.Descriptor_Sets;

namespace Engine.Components
{
    public class ModelComponent : Component
    {
       /* public Material[] Materials;
        public List<EmbeddedTexture> Textures;
        public List<Animation> Animations;
        public List<Mesh> Meshes;*/

        public VertexBuffer<float> myVertexBuffer;
        public IndexBuffer<uint> myIndexBuffer;
        public BoundingBox MeshWidthHeight;
        public float[][] ModelIDToVertices;//= new Dictionary<int, float[]>();
        public uint[][] ModelIDToIndices;
        public String PipelineID = "Default";
        public Material[] myMaterials;
        internal bool hasTransparency;

        public override void CopyFrom<T>(T aComponent)
        {
            ModelComponent myIncomingComponent = aComponent as ModelComponent;
            

            if (myIncomingComponent == null) throw new INVALID_COMPONENT_COPY_ATTEMPT(typeof(T));
            //Materials = new Material[myIncomingComponent.Materials.Length];
            //Array.Copy(myIncomingComponent.Materials, Materials, myIncomingComponent.Materials.Length);
            //Textures = new List<EmbeddedTexture>(myIncomingComponent.Textures);
            //Animations = new List<Animation>(myIncomingComponent.Animations);
            //Meshes = new List<Mesh>(myIncomingComponent.Meshes);
            CopyModelIndicies(myIncomingComponent);
            CopyModelVerticies(myIncomingComponent);


        }



        private void CopyModelVerticies(ModelComponent myIncomingComponent)
        {
            ModelIDToVertices = new float[myIncomingComponent.ModelIDToIndices.Length][]; 
            Parallel.For(0, myIncomingComponent.ModelIDToIndices.Length, delegate (int i)
            {
                ModelIDToVertices[i] = new float[myIncomingComponent.ModelIDToVertices[i].Length];
                Array.Copy(myIncomingComponent.ModelIDToVertices[i], ModelIDToVertices[i], ModelIDToVertices[i].Length);
            });
        }

        private void CopyModelIndicies(ModelComponent myIncomingComponent)
        {
            ModelIDToIndices = new uint[myIncomingComponent.ModelIDToIndices.Length][];
            Parallel.For(0, myIncomingComponent.ModelIDToIndices.Length, delegate (int i)
            {
                ModelIDToIndices[i] = new uint[myIncomingComponent.ModelIDToIndices[i].Length];
                Array.Copy(myIncomingComponent.ModelIDToIndices[i], ModelIDToIndices[i], ModelIDToIndices[i].Length);
            });
        }


    }
}
