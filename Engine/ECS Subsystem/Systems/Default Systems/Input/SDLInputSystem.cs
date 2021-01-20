using Engine.ECS.Components.Input_Components;
using Engine.ECS.Scheduler.Threading;
using Engine.ECS.Systems.System_Types;
using Engine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS.Systems.Default_Systems.Input
{
    [SystemWriter(typeof(InputManagerComponent))]
    public class SDLInputSystem : SingletonComponentUpdateSystem<InputManagerComponent>
    {
        public SDLInputSystem(World BaseWorld) : base(BaseWorld)
        {
        }

        public override void Postprocess()
        {

        }

        public override void Preprocess()
        {

        }


        protected override void Process(ThreadController myController)
        {
            SDL2.SDL.SDL_Event myEvent = new SDL2.SDL.SDL_Event();
            
            while (SDL2.SDL.SDL_PollEvent(out myEvent) == 1)
            {
                switch (myEvent.type)
                {
                    case SDL2.SDL.SDL_EventType.SDL_FIRSTEVENT:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_QUIT:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_WINDOWEVENT:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_SYSWMEVENT:
                        break;

                    case SDL2.SDL.SDL_EventType.SDL_TEXTEDITING:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_TEXTINPUT:
                        break;
                  
                    case SDL2.SDL.SDL_EventType.SDL_JOYAXISMOTION:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_JOYBALLMOTION:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_JOYHATMOTION:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_JOYBUTTONDOWN:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_JOYBUTTONUP:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_JOYDEVICEADDED:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_JOYDEVICEREMOVED:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_CONTROLLERAXISMOTION:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_CONTROLLERBUTTONDOWN:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_CONTROLLERBUTTONUP:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_CONTROLLERDEVICEADDED:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_CONTROLLERDEVICEREMAPPED:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_FINGERDOWN:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_FINGERUP:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_FINGERMOTION:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_DOLLARGESTURE:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_DOLLARRECORD:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_MULTIGESTURE:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_CLIPBOARDUPDATE:
                        break;

                    case SDL2.SDL.SDL_EventType.SDL_DROPFILE:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_RENDER_TARGETS_RESET:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_USEREVENT:
                        break;
                    case SDL2.SDL.SDL_EventType.SDL_LASTEVENT:
                        break;
                }
            }
        }
    }
}
