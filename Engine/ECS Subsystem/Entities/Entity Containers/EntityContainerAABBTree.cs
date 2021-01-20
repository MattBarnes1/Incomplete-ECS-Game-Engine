using Engine.Components;
using Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public class EntityContainerAABBTree<T> : EntityContainer<T> where T : EntityReference, new()
    {
        AABBTree myTree = new AABBTree();
        public override int Count { get { 
                if (myItems == null) return 0; 
                else return myItems.Count; } }

        public override bool IsReadOnly { get { return false; } }

        public override T this[int index] { 
            get
            {
                if(myItems == null)
                {
                    return null;
                } else
                {
                    return myItems[index];
                }
            } 
        }

        public EntityContainerAABBTree()
        {
        }
        RayCastHit<T> myItems;
        public IReadOnlyList<T> Raycast(Vector3 Start, Vector3 Direction)
        {
           return (IReadOnlyList<T>)myTree.Raycast<T>(new Ray(Start, Direction));
        }

        protected override void Add(T item)
        {
            IReadOnlyList<TransformComponent> TransformComponents = item.Get<TransformComponent>();
            IReadOnlyList <ModelComponent> ModelComponent = item.Get<ModelComponent>();
            BoundingBox aBox = new BoundingBox(ModelComponent[0], TransformComponents[0]);
            myTree.Insert<T>(ref aBox, item); 
        }

        public override void Clear()
        {
            myTree.Clear();
        }

        public override bool Contains(T item)
        {
            return myTree.Contains(item);
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }


        private void EntityPositionChanged(Entity item)
        {
            Remove(item);
            Add(item);
        }
        private void EntityModelChanged(Entity item)
        {
            Remove(item);
            Add(item);
        }

        public override void Sort()
        {

        }

        protected override bool Remove(T item)
        {
            var TransformComponents = item.Get<TransformComponent>();
            var ModelComponent = item.Get<ModelComponent>();
            if (ModelComponent == null || TransformComponents == null) return false;
            BoundingBox aBox = new BoundingBox(ModelComponent[0], TransformComponents[0]);
            return myTree.Remove<T>(item);
        }
    }
}
