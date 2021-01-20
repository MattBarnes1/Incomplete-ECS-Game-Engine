using Engine.Components;


using Engine.Systems;
using Engine.Resources;
using System;
using Engine.DataTypes;
using Engine.System_Types;
using Engine.Entities;
using Engine.Components.Base;
using System.Numerics;
using Engine.ECS.Components.Input_Components;
using Engine.Utilities;

namespace Engine.Default_Systems
{
    [SystemReader(typeof(TextureComponent), typeof(CursorComponent), typeof(MouseInputComponent))]
    [SystemWriter(typeof(TransformComponent))]
    public class CursorSystem : OneToManySystem<MouseInputComponent,EntityReference<TextureComponent, TransformComponent, CursorComponent>>
    {
        public CursorSystem(World MyBase) : base(MyBase, 0)
        {
        }



        static String TextureCursorIsChangingTo;
        static object Lock = new object();
        public static void SetCursor(String texture2D)
        {
            lock(Lock)
            {
                TextureCursorIsChangingTo = texture2D;
            }
        }

        public static void LoadCursor(String texture2D)
        {
            Entity myEntity = new Entity();
            TextureComponent myImage = myEntity.AddComponent<TextureComponent>();
            ResourceManager.LoadInto<TextureComponent>(myImage, "GUIImages/gauntlet");
            ModelComponent myModel = myEntity.AddComponent<ModelComponent>();
            GeometryUtility.SetModelVertices(myModel, Shape.Square, new Vector2(myImage.Width, myImage.Height));
            TransformComponent myTrans = myEntity.AddComponent<TransformComponent>();

            myTrans.DrawLayer = 1;

            myEntity.AddComponent<CursorComponent>();
        }

        public override void Preprocess()
        {

        }

        public override void Postprocess()
        {

        }

        protected override void Process(MouseInputComponent myReferenceSingle, EntityReference<TextureComponent, TransformComponent, CursorComponent> myReferencedMany)
        {
            if (TextureCursorIsChangingTo != null)
            {
                lock (Lock)
                {
                    ResourceManager.LoadInto<TextureComponent>(myReferencedMany.Component1[0], TextureCursorIsChangingTo);
                    TextureCursorIsChangingTo = null;
                }
            }
            myReferencedMany.Component2[0].WorldPosition = myReferenceSingle.MousePosition;
        }
    }
}
