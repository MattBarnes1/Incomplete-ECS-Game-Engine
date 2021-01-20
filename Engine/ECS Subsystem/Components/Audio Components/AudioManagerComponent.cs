using Engine.ECS.Components.Base;
using Engine.LowLevelInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS_Subsystem.Components.Audio_Components
{
    public class AudioManager
    {
        public AudioManagerComponent()
        {
            SDL2.SDL.SDL_InitSubSystem((uint)SDL_INIT_FLAGS.SDL_INIT_AUDIO);
        }


    }
}
