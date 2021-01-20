using Engine.ECS.Components.Base;
using Engine.LowLevelInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace Engine.ECS.Components.Input_Components
{
    public class InputManagerComponent : SingletonComponent<InputManagerComponent>
    {
        private SDL_EventFilter myFilterBase;

        public InputManagerComponent()
        {
            //Informs SDL that this Subsystem is Active.
            SDL2.SDL.SDL_InitSubSystem((uint)SDL_INIT_FLAGS.SDL_INIT_EVENTS);
            myFilterBase = new SDL_EventFilter(InputFilter);
            SDL_SetEventFilter(myFilterBase, IntPtr.Zero);
        }

        public void RegisterEventFilter(SDL_EventFilter mouseInputFilter)
        {
            myFilter += mouseInputFilter;
        }

        private int InputFilter(IntPtr userdata, IntPtr sdlevent)
        {
            if (myFilter != null)
            {
                foreach (var A in myFilter.GetInvocationList())
                {
                    if (((int)A.DynamicInvoke(userdata, sdlevent)) != 1)
                    {
                        break;
                    }
                }
            }

            return 0;
        }


        static SDL_EventFilter myFilter { get; set; }


        public float MousePosition { get; internal set; }
    }
}
