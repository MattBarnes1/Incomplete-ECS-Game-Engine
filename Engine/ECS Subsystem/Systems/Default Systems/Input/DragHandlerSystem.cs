using Engine.Components;
using Engine.ECS.Components;
using Engine.ECS.Components.Input_Components;
using Engine.Entities;
using Engine.System_Types;
using Engine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Default_Systems
{
    [SystemReader(typeof(MouseInputComponent))]
    public class DragHandlerSystem : SingletonToManySystem<MouseInputComponent, EntityReference<DragReceieverComponent, ModelComponent, TransformComponent>>
    {
        public DragHandlerSystem(World myBase) : base(myBase, 0)
        {
        }

        public override void Postprocess()
        {

        }

        public override void Preprocess()
        {

        }
        protected override void Process(EntityReference<DragReceieverComponent, ModelComponent, TransformComponent> myReferenceSingle)
        {

        }
    }
}
