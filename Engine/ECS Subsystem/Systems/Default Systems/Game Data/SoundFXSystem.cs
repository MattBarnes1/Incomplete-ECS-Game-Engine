using Engine.Components;
using Engine.Entities;
using Engine.System_Types;
using Engine.Systems;

using EngineData.Engine.Components;
using EngineData.Engine.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Default_Systems
{
    public class SoundFXSystem : ManyToManySystem<EntityReference<CameraComponent>, EntityReference<SoundFXComponent, TransformComponent>>
    {
        public SoundFXSystem(World myWorld) : base(myWorld, 0)
        {
        }

        public override void Postprocess()
        {

        }

        public override void Preprocess()
        {

        }

        protected override void Process(EntityReference<CameraComponent> myFirstLeft, EntityContainer<EntityReference<SoundFXComponent, TransformComponent>> myIteratorTwo)
        {

        }
    }
}
