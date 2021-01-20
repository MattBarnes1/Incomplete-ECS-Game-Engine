
using Engine.Components;
using Engine.Components.Base;
using Engine.DataTypes;
using Engine.Resources;
using Engine.Utilities;
using System;
using System.Numerics;

namespace Engine.Entities
{
    public class Canvas : Entity
    {

        public Canvas(String Background, Vector2 WidthHeight, Vector2 Position = default, bool Draggable = true)
        {
            WindowComponent myWindow = AddComponent<WindowComponent>();
            TransformComponent myTrans = AddComponent<TransformComponent>();
            TextureComponent myBackground = AddComponent<TextureComponent>();
            ModelComponent myModel = AddComponent<ModelComponent>();
            myModel.PipelineID = "GUIDefault";
            GeometryUtility.SetModelVertices(myModel, Shape.Square, WidthHeight);
            myWindow.CanDrag = Draggable;
            myTrans.DrawLayer = 0.9f;
            if (Background != null)
            {
                ResourceManager.LoadInto<TextureComponent>(myBackground, Background);
                myTrans.WorldPosition = new Vector3((int)Position.X, (int)Position.Y, 0);
            }
        }
    }
}
