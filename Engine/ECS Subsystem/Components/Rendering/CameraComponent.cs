using Engine.DataTypes;
using Engine.Renderer.Vulkan;
using System;
using System.Numerics;

namespace Engine.Components
{
    public class CameraComponent : Component
    {
        public Vector3 CameraTarget { get; set; } = new Vector3(0,0,0);
        public Matrix4x4 LookAtMatrix { get; set; }
        public bool Active { get; set; }
        public BoundingFrustum myViewingFustrum { get; set; }
        public FrameBuffer myRenderTargetBuffer { get; set; }
        public Vector3 UpDirection { get; } = new Vector3(0, 1, 0);
        public Vector3 CameraPosition { get; private set; }

        public CameraComponent()
        {
            CameraPosition = new Vector3(4, 3, -1); 
            //TODO: Viewing fustrum
           // areaBuffer.View = GetLookAtMatrix();
           // areaBuffer.Proj = Matrix4x4.CreatePerspective(0.785398f, VulkanRenderer.SelectedPhysicalDevice.SurfaceCapabilities.CurrentExtent.Width / VulkanRenderer.SelectedPhysicalDevice.SurfaceCapabilities.CurrentExtent.Height, 0.1f, 10000.0f);
        }



        public Matrix4x4 GetLookAtMatrix()
        {
            
            return Matrix4x4.CreateLookAt(CameraPosition, CameraTarget, UpDirection);
        }

        internal bool isInViewingFustrum(Vector3 worldPosition)
        {
            return true;//TODO:
        }
    }
}
