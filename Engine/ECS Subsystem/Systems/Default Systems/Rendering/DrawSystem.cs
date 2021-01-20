using System;
using System.Collections.Generic;
using Engine.Components;
using Engine.Systems;



using EngineRenderer;
using Engine.Entities;
using Engine.System_Types;
using Engine.Renderer.Vulkan;
using Engine.Low_Level.Vulkan.Classes_By_Me.Buffer;
using System.Numerics;
using Vulkan;
using Engine.Renderer.Vulkan.Descriptor_Sets;

namespace Engine
{
    [SystemReader(typeof(TextureComponent), typeof(TransformComponent), typeof(ModelComponent))]
    [SystemWriter(typeof(RendererManagerComponent))]
    public class DrawSystem : SingletonToManySystem<RendererManagerComponent, EntityReference<ModelComponent, TextureComponent, TransformComponent>>
    {
        public DrawSystem(World MyBase ) : base(MyBase, Int32.MaxValue)
        {
        }

        public override void Postprocess()
        {
            SingletonInstance.myRenderer.myEngineToUse.RenderEnd();

        }

        public override void Preprocess()
        {
            SingletonInstance.myRenderer.myEngineToUse.RenderStart();
        }

        protected override void Process(EntityReference<ModelComponent, TextureComponent, TransformComponent> myReferenceSingle)
        {
            if (base.SingletonInstance.myRenderEngine.ActiveCamera != null)//TODO: this is extremely inefficient
            {

                var myModel = myReferenceSingle.Component1[0];
                var myTexture = myReferenceSingle.Component2[0];
                var myTransform = myReferenceSingle.Component3[0];
                if (myTexture != null)
                {
                    if (myTexture.ImageBytes != null && myTexture.TextureImage == null)
                    {

                    }
                    else if (myTexture.ImageBytes == null)
                    {
                        throw new Exception("No imagebytes present.");
                    }
                }
                if (myModel.ModelIDToVertices == null)
                    throw new Exception("Invalid Model!");
                if (myModel.myVertexBuffer == null)
                    myModel.myVertexBuffer = new VertexBuffer<float>(myModel.ModelIDToVertices[0]);
                if (myModel.myIndexBuffer == null)
                    myModel.myIndexBuffer = new IndexBuffer<uint>(myModel.ModelIDToIndices[0]);
               
                //TODO: update model bounding box etc here.

                base.SingletonInstance.myRenderEngine.DrawWorldSpace(myReferenceSingle.ID, myReferenceSingle.Component1[0], myReferenceSingle.Component2[0], myReferenceSingle.Component3[0]);
            }
       
        }
    }
}
