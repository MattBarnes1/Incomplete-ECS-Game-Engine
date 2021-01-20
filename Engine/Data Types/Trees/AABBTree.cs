using Engine.Entities;
using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.DataTypes
{
    public class AABBTree
    {
        public int Count { get { return myNodes.Count; } }




        internal class AABBNode
        {
            internal AABBNode Parent;
            internal AABBNode Left;
            internal AABBNode Right;
            internal int DepthOffset;
            internal BoundingBox myBox;
            public virtual bool ContainsData() { return false; }
        }

        internal class AABBNodeObject<T> : AABBNode
        {
            public override bool ContainsData() { return true; }
            public T myData { get; set; }
        }

        internal AABBNode Root;

        public AABBTree()
        {

        }
        /// <summary>
        /// This currently runs at an O(1) complexity but more memory usage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myItem"></param>
        /// <returns></returns>
        public bool Contains<T>(T myItem)
        {
            return myNodes.ContainsKey((object)myItem);
        }

        internal void Clear()
        {
            myNodes.Clear();
            Root = null;
        }


        /*Branch – Our branches always have exactly two children (known as left and right) and are assigned an AABB that is large enough to contain all of it’s descendants.
Leaf – Our leaves are associated with a game world object and through that have an AABB. A leaf’s AABB must fit entirely within it’s parents AABB and due to how our branches are defined that means it fits in every ancestors AABB.
Root – Our root may be a branch or a leaf*/


        public void Insert<T>(ref BoundingBox myBox, T input)
        {
            AABBNode myNewNode = new AABBNodeObject<T>() { myBox = myBox, myData = input };
            myNodes.Add(input, myNewNode);
            if (Root == null)
            { 
                Root = myNewNode; 
                return;
            }
            AABBNode CurrentObject = Root;
            while (CurrentObject != null)
            {
                if (CurrentObject.ContainsData()) //It's a game object
                {
                    MergeIntoBranch(myNewNode, CurrentObject);
                    return;
                }
                else //It's just a branch.
                {
                    ResizeBranch(myNewNode, CurrentObject);
                    ContainmentType leftContainment = ContainmentType.Disjoint;
                    ContainmentType rightContainment = ContainmentType.Disjoint;
                    if (CurrentObject.Left != null)
                        leftContainment = CurrentObject.Left.myBox.Contains(myBox);
                    if (CurrentObject.Right != null)
                        rightContainment = CurrentObject.Right.myBox.Contains(myBox);

                    if ((leftContainment == ContainmentType.Contains || leftContainment == ContainmentType.Intersects) && rightContainment == ContainmentType.Disjoint)
                    {
                        CurrentObject = CurrentObject.Left;
                    }
                    else if ((rightContainment == ContainmentType.Contains || rightContainment == ContainmentType.Intersects) && leftContainment == ContainmentType.Disjoint)
                    {
                        CurrentObject = CurrentObject.Right;
                    }
                    else if (leftContainment == ContainmentType.Intersects && rightContainment == ContainmentType.Intersects)
                    {
                        CurrentObject = GetNextByHeuristic(myNewNode, CurrentObject);
                    }
                    else
                    {
                        throw new Exception("Remove or Insert failed to clean up AABB tree properly!");
                    }
                }
            }
        }

        internal bool Remove<T>(BoundingBox aBox)
        {
            throw new NotImplementedException();
        }

        private static void ResizeBranch(AABBNode myNewNode, AABBNode CurrentObject)
        {
            var ContainmentTypeReturned = CurrentObject.myBox.Contains(myNewNode.myBox);
            if (ContainmentTypeReturned == ContainmentType.Intersects || ContainmentTypeReturned == ContainmentType.Disjoint)
            {
                BoundingBox.CreateMerged(ref myNewNode.myBox, ref CurrentObject.myBox, out CurrentObject.myBox);
            }
        }

        private AABBNode GetNextByHeuristic(AABBNode myNewNode, AABBNode CurrentObject)
        {
            float DistanceLeft = -1;
            float DistanceRight = -1;
            DistanceLeft = DistanceApproximation.DistanceTaxiCab(CurrentObject.Left.myBox.Center, myNewNode.myBox.Center);
            DistanceRight = DistanceApproximation.DistanceTaxiCab(CurrentObject.Right.myBox.Center, myNewNode.myBox.Center);
            AABBNode Closest;
            if (DistanceLeft >= DistanceRight)
            {
                CurrentObject = CurrentObject.Right;
            }
            else
            {
                CurrentObject = CurrentObject.Left;
            }

            return CurrentObject;
        }

        private void MergeIntoBranch(AABBNode myNewNode, AABBNode currentObject)
        {
            AABBNode myNewBranch = new AABBNode();
            BoundingBox.CreateMerged(ref myNewNode.myBox, ref currentObject.myBox, out myNewBranch.myBox);
            myNewBranch.Left = currentObject;
            myNewBranch.DepthOffset = currentObject.DepthOffset;
            currentObject.DepthOffset--;
            myNewBranch.Right = myNewNode; //Depth Offset = 0;
            myNewNode.Parent = myNewBranch;
            if (currentObject.Parent == null)
            {
                Root = myNewBranch;
            }
            else if (currentObject.Parent.Right == currentObject)
            {
                currentObject.Parent.Right = myNewBranch;
            }
            else if (currentObject.Parent.Left == currentObject)
            {
                currentObject.Parent.Left = myNewBranch;
            }
            myNewBranch.Parent = currentObject.Parent;
            currentObject.Parent = myNewBranch;
        }

        Dictionary<Object, AABBNode> myNodes = new Dictionary<Object, AABBNode>(); //TODO: Custom hashing



       

        public bool Remove<T>(T MyItem)
        {
            AABBNode NodeData;
            if (myNodes.TryGetValue((Object)MyItem, out NodeData))
            {
                //Cases No children, a child, two children -> never happens can't delete branches here
                //First Handle My Node Removed Children.
                //Cases Parent only has this as child, Parent
                var Parent = NodeData.Parent;
                if (NodeData.Parent.Left == NodeData)
                {
                    AABBNode LastNodeRemoved;
                    if (NodeData.Parent.Right == null)
                    {
                        NodeData.Parent.Left = null;
                        int InvalidatedParentCount = RemoveInvalidParents(NodeData.Parent, out LastNodeRemoved);
                    }
                    else
                    {
                        NodeData.Parent.DepthOffset -= 1;
                    }
                }
                else
                {
                    NodeData.Parent.Right = null;
                    if (NodeData.Parent.Left == null) //case 1 node;
                    {
                        NodeData.Parent.Right = null;
                        AABBNode LastNodeRemoved;
                        int InvalidatedParentCount = RemoveInvalidParents(NodeData.Parent, out LastNodeRemoved);

                    }
                    else
                    {
                        NodeData.Parent.DepthOffset -= 1;
                    }
                }
                myNodes.Remove((Object)MyItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        private int RemoveInvalidParents(AABBNode NodeBeingRemoved, out AABBNode lastModifiedNode)
        {
            int Count = 0;
            var myParent = NodeBeingRemoved.Parent;

            if (NodeBeingRemoved.Parent.Left == NodeBeingRemoved)
            {
                NodeBeingRemoved.Parent.Left = null;
            }
            else if (NodeBeingRemoved.Parent.Right == NodeBeingRemoved)
            {
                NodeBeingRemoved.Parent.Right = null;
            }
            lastModifiedNode = NodeBeingRemoved.Parent;


            if (myParent.Right == null && myParent.Left == null) //case 1 node;
            {
                var formerChildNode = myParent;
                myParent = myParent.Parent;
                while (myParent != null)
                {
                    if (myParent.Left == formerChildNode)
                    {
                        myParent.Left = null;
                        if (myParent.Right == null)
                        {
                            formerChildNode = myParent;
                            myParent = myParent.Parent;
                            Count++;
                        }
                        else
                        {
                            lastModifiedNode = myParent;
                            myParent = null;
                        }
                    }
                    else
                    {
                        myParent.Right = null;
                        if (myParent.Left == null)
                        {
                            Count++;
                            formerChildNode = myParent;
                            myParent = myParent.Parent;
                        }
                        else
                        {
                            lastModifiedNode = myParent;
                            myParent = null;
                        }
                    }
                }
            }
            else
            {
                lastModifiedNode = myParent;
            }
            return Count;
        }


        public RayCastHit<T> Raycast<T>(Ray aRay)
        {
            if (Root == null) return new RayCastHit<T>(new List<T>());
            List<T> myResults = new List<T>();
            Stack<AABBNode> NodesToExplore = new Stack<AABBNode>();
            AABBNode Current = Root;
            while(Current != null)
            {
                if(aRay.Intersects(Current.Left.myBox).HasValue) //So it hit.
                {
                    if(Current.Left.ContainsData() && Current.Left is AABBNodeObject<T>)
                    {
                        myResults.Add(((AABBNodeObject<T>)Current.Left).myData);
                    } else
                    {
                        NodesToExplore.Push(Current.Left);
                    }
                }
                if (aRay.Intersects(Current.Right.myBox).HasValue) //So it hit.
                {
                    if (Current.Right.ContainsData() && Current.Right is AABBNodeObject<T>)
                    {
                        myResults.Add(((AABBNodeObject<T>)Current.Right).myData);
                    }
                    else
                    {
                        NodesToExplore.Push(Current.Right);
                    }
                }
                if(NodesToExplore.Count == 0)
                {
                    break; 
                }
                Current = NodesToExplore.Pop();
            }
            return new RayCastHit<T>(myResults);
        }
        public RayCastHit<T> RaycastReturnFirst<T>(Ray aRay)
        {
            if (Root == null) return new RayCastHit<T>(new List<T>());
            List<T> myResults = new List<T>();
            Stack<AABBNode> NodesToExplore = new Stack<AABBNode>();
            AABBNode Current = Root;
            while (Current != null)
            {
                if (aRay.Intersects(Current.Left.myBox).HasValue) //So it hit.
                {
                    if (Current.Left.ContainsData() && Current.Left is AABBNodeObject<T>)
                    {
                        myResults.Add(((AABBNodeObject<T>)Current.Left).myData);
                        break;
                    }
                    else
                    {
                        NodesToExplore.Push(Current.Left);
                    }
                }
                if (aRay.Intersects(Current.Right.myBox).HasValue) //So it hit.
                {
                    if (Current.Right.ContainsData() && Current.Right is AABBNodeObject<T>)
                    {
                        myResults.Add(((AABBNodeObject<T>)Current.Right).myData);
                        break;
                    }
                    else
                    {
                        NodesToExplore.Push(Current.Right);
                    }
                }
                if (NodesToExplore.Count == 0)
                {
                    break;
                }
                Current = NodesToExplore.Pop();
            }
            return new RayCastHit<T>(myResults);
        }
        public RayCastHit<T> BoundingPrimativeCast<T>(BoundingBox myTestBox)
        {
            if (Root == null) return new RayCastHit<T>(new List<T>());
            List<T> myResults = new List<T>();
            Stack<AABBNode> NodesToExplore = new Stack<AABBNode>();
            AABBNode Current = Root;
            while (Current != null)
            {
                ContainmentType left = ContainmentType.Disjoint;
                ContainmentType right = ContainmentType.Disjoint;
                if (Current.Left != null)
                    left = myTestBox.Contains(Current.Left.myBox);
                if (Current.Right != null)
                    right = myTestBox.Contains(Current.Right.myBox);
                if (left == ContainmentType.Contains || left == ContainmentType.Intersects) //So it hit.
                {
                    if (Current.Left.ContainsData())
                    {
                        myResults.Add(((AABBNodeObject<T>)Current.Left).myData);
                    }
                    else
                    {
                        NodesToExplore.Push(Current.Left);
                    }
                }
                if (right == ContainmentType.Contains || right == ContainmentType.Intersects) //So it hit.
                {
                    if (Current.Right.ContainsData())
                    {
                        myResults.Add(((AABBNodeObject<T>)Current.Right).myData);
                    }
                    else
                    {
                        NodesToExplore.Push(Current.Right);
                    }
                }
                if (NodesToExplore.Count == 0)
                {
                    break;
                }
                Current = NodesToExplore.Pop();
            }
            return new RayCastHit<T>(myResults);
        }

        public void Validate()
        {

        }
    }
}
