

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Engine.Components.Base
{
    public class WindowComponent : Component
    {
        public string Name = "Window";
        public bool CanDrag = true;// { get; set; }
        public Guid Handle { get; internal set; }
        public bool Interactable;
        public bool Hidden;
        public bool RequestClosing;

        public WindowComponent()
        {
            Handle = Guid.NewGuid();
        }
    }
}
