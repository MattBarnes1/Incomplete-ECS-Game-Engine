using Engine.Components;
using Engine.Components.Base;
using Engine.DataTypes;
using Engine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities.Iterators
{
    public class WindowEntityIterator : EntityIterator<EntityReference<WindowComponent, ModelComponent, TransformComponent>>
    {
        EntityContainerAABBTree<EntityReference<WindowComponent, ModelComponent, TransformComponent>> myTree;
        public WindowEntityIterator(World myBase) : base(myBase, new EntityContainerAABBTree<EntityReference<WindowComponent, ModelComponent, TransformComponent>>())
        {
            myTree = (EntityContainerAABBTree<EntityReference<WindowComponent, ModelComponent, TransformComponent>>)base.GetContainer();


        }

        public override EntityReference<WindowComponent, ModelComponent, TransformComponent> Current
        {
            get
            {
                HasNext = false;
                return myCurrent;
            }
        }

        bool HasNext = false;

        EntityReference<WindowComponent, ModelComponent, TransformComponent> myCurrent;

        public void FindClickedWindow(Vector3 myPoint)
        {
            if (base.Count == 0)
            {
                HasNext = false;
                myCurrent = null;
            } 
            else
            {
               IReadOnlyList<EntityReference<WindowComponent, ModelComponent, TransformComponent>> myList = myTree.Raycast(new Vector3(myPoint.X, myPoint.Y, 0), new Vector3(0, 0, 1));
                if (myList.Count == 0)
                {
                    HasNext = false;
                    myCurrent = null;
                } else
                {
                    myCurrent = myList[0];
                    HasNext = true;
                }
            }
        }

        protected override void ProcessEntityChange(Entity myEntity, EntityBits ComponentBits)
        {
            base.ProcessEntityChange(myEntity, ComponentBits);
        }

        public override bool hasNext()
        {
            return HasNext;
        }
    }
}
