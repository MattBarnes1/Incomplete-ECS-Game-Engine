using Engine.Components;
using Engine.Entities;
using Engine.System_Types;
using Engine.Systems;
using EngineRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS.Systems.Default_Systems
{
    [SystemWriter(typeof(CameraComponent), typeof(RendererManagerComponent))]
    [SystemReader(typeof(TransformComponent), typeof(CameraComponent))]
    public class CameraControllerSystem : OneToManySystem<RendererManagerComponent, EntityReference<CameraComponent, TransformComponent>>
    {
        public CameraControllerSystem(World myWorld) : base(myWorld, 0)
        {
        }

        public override void Postprocess()
        {

        }

        public override void Preprocess()
        {

        }


        protected override void Process(RendererManagerComponent myReferenceSingle, EntityReference<CameraComponent, TransformComponent> myReferencedMany)
        {
          /*  if (myReferencedMany.Component1[0].Active && myReferenceSingle.ActiveCameraID != myReferencedMany.ID)
            {
                myReferenceSingle.ActiveCameraID = myReferencedMany.ID;
                if (myReferenceSingle.ActiveCamera != null)
                    myReferenceSingle.ActiveCamera.Active = false;
                myReferenceSingle.ActiveCamera = myReferencedMany.Component1[0];
                myReferenceSingle.myRenderer.myEngineToUse.SetCamera(myReferencedMany.Component1[0]);
            }*/
        }
    }
}
