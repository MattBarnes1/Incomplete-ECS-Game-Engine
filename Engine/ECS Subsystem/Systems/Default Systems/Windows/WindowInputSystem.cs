using Engine.Components;
using Engine.Components.Base;
using Engine.Entities;
using Engine.Entities.Iterators;
using Engine.System_Types;
using Engine.Systems;



using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.Default_Systems
{
    [SystemReader(typeof(MouseInputComponent), typeof(TransformComponent), typeof(ModelComponent))]
    [SystemWriter(typeof(WindowComponent))]
    public class WindowInputSystem : SingletonToManySystem<MouseInputComponent, EntityReference<WindowComponent, ModelComponent, TransformComponent>>
    {

        public WindowInputSystem(World MyBase) : base(MyBase, new WindowEntityIterator(MyBase), 0)
        {            

        }

        public override void Postprocess()
        {

        }

        public override void Preprocess()
        {
           var Container = (WindowEntityIterator)base.GetEntityIterator();
            Container.FindClickedWindow(this.SingletonInstance.MousePosition);
        }

        protected override void Process(EntityReference<WindowComponent, ModelComponent, TransformComponent> myReferenceSingle)
        {

        }


        /*  List<EntityReference<WindowComponent, TransformComponent, MouseInputComponent>> Possible = new List<EntityReference<WindowComponent, TransformComponent, MouseInputComponent>>();
          EntityReference<WindowComponent, TransformComponent, MouseInputComponent> LastActive = null;
          //
          //Find Top Most window.
          foreach (var A in myMany)
          {

              if(A.Component2[0].DrawLayer == 0.9f && LastActive == null)
              {
                  LastActive = A;
              }
              if(A.Component3[0].LeftButtonClicked)
              {
                  if(A.Component2[0].myTransformWorldRectangle.Contains(A.Component3[0].MousePosition))
                  {
                      Possible.Add(A);
                  }
              }
          }

          if (Possible.Count > 0 && LastActive != null)
          {
              if (LastActive.Component3[0].LeftButtonClicked)
              {
                  Possible.Sort(delegate (EntityReference<WindowComponent, TransformComponent, MouseInputComponent> A, EntityReference<WindowComponent, TransformComponent, MouseInputComponent> B)
                  {
                      return A.Component2[0].DrawLayer.CompareTo(B.Component2[0].DrawLayer); //Only looks for window containers that contain the mouse.
                  });

                  if (LastActive.Component2 == Possible[0].Component2)
                      return;

                  float Last = LastActive.Component2[0].DrawLayer;

                  LastActive.Component2[0].DrawLayer = 0.1f;
                  Possible[0].Component2[0].DrawLayer = Last;
              }
          }
          else if (LastActive != null && LastActive.Component3[0].Dragging && LastActive.Component2[0].myTransformWorldRectangle.Contains(LastActive.Component3[0].MousePosition) && LastActive.Component1[0].CanDrag)
          {
              var Difference = (LastActive.Component3[0].MousePosition.ToVector2() - LastActive.Component3[0].OldMousePosition.ToVector2());
              Debug.WriteLine("Diff: " + Difference);
              LastActive.Component2[0].myTransformWorldRectangle.Offset(Difference.X, Difference.Y);
              Debug.WriteLine(LastActive.Component2[0].myTransformWorldRectangle.Location);
          }*/
    }
}
