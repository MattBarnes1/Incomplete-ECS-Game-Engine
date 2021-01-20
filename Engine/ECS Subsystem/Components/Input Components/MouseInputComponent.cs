

using Engine.Data_Types;
using Engine.DataTypes;
using Engine.ECS.Components.Base;
using System.Numerics;

namespace Engine.Components
{
    public class MouseInputComponent : SingletonComponent<MouseInputComponent>
    {

        public Vector3 MousePosition { get; set; }

        public ButtonState LeftButtonState;
        public ButtonState RightButtonState;
        internal ButtonState MiddleButtonState;

        public Point OldMousePosition { get; set; }
        public bool Dragging { get; set; }


    }
}
