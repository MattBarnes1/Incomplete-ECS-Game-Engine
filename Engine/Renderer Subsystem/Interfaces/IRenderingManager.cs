using Engine.Components;
using Engine.DataTypes;
using Engine.DataTypes.Texture_Data;
using Engine.Renderer.Vulkan.Descriptor_Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineRenderer.Interfaces
{
    public interface IRenderingManager
    {
        CameraComponent ActiveCamera { get; }

        void RenderStart();
        void RenderEnd();
        void DrawWorldSpace(uint ID, ModelComponent myModel, TextureComponent myTexture, TransformComponent myRectangle);

        void SetCamera(CameraComponent T);
    }
}
