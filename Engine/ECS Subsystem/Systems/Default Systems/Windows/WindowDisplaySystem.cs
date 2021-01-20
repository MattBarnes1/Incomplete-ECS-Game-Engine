using Engine.Components;
using Engine.Components.Base;
using Engine.Entities;
using Engine.System_Types;
using Engine.Systems;


namespace Engine.Default_Systems
{
    [SystemWriter(typeof(TextureComponent))]
    [SystemReader(typeof(WindowComponent))]
    public class WindowDisplaySystem : SelfContainedEntitySystem<EntityReference<WindowComponent, TextureComponent, TransformComponent>>
    {
        public WindowDisplaySystem(World myBase, int Priority) : base(myBase, Priority)
        {

        }

        public override void Postprocess()
        {

        }

        public override void Preprocess()
        {

        }

        protected override void Process(EntityReference<WindowComponent, TextureComponent, TransformComponent> myEntitySet)
        {
            for(int i = 0; i < myEntitySet.Component1.Count; i++)
            {
                if (myEntitySet.Component1[i].RequestClosing)
                {
                    myEntitySet.myEntity.Destroy();
                }
                else if (myEntitySet.Component1[i].Hidden)
                {
                    myEntitySet.Component2[i].isVisible = false;
                }
            }
        }
    }
}
