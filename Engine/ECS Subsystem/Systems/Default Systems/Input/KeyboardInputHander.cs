using Engine.Components;
using Engine.Components.Base;
using Engine.ECS.Components.Input_Components;
using Engine.ECS.Scheduler.Threading;
using Engine.ECS.Systems.System_Types;
using Engine.Entities;
using Engine.System_Types;
using Engine.Systems;

using SDL2;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace Engine.Default_Systems
{
    [SystemWriter(typeof(KeyboardInputComponent))]
    public class KeyboardInputSystem : SingletonComponentUpdateSystem<KeyboardInputComponent>
    {

        ConcurrentQueue<SDL2.SDL.SDL_Event> myEvent = new ConcurrentQueue<SDL.SDL_Event>();
        public KeyboardInputSystem(World MyBase) : base(MyBase)
        {
            InputManagerComponent myComponent = InputManagerComponent.Get();
            myComponent.RegisterEventFilter(KeyboardInputFilter);
        }

        public int KeyboardInputFilter(IntPtr userdata, IntPtr sdlevent)
        {
            var Result = (SDL2.SDL.SDL_Event)Marshal.PtrToStructure(sdlevent, typeof(SDL2.SDL.SDL_Event));
            switch (Result.type)
            {
                case SDL2.SDL.SDL_EventType.SDL_KEYDOWN:
                    myEvent.Enqueue(Result);
                    return 0;
                case SDL2.SDL.SDL_EventType.SDL_KEYUP:
                    myEvent.Enqueue(Result);
                    return 0;
                default:
                    return 1;
            }
        }

        public override void Preprocess()
        {
        }

        public override void Postprocess()
        {
            myEvent = new ConcurrentQueue<SDL.SDL_Event>();
        }

        protected override void Process(ThreadController myController)
        {
            myController.AddDelegate(delegate (Object obj)
            {
                while (myEvent.Count != 0)
                {
                    if (myEvent.TryDequeue(out var aResult))
                    {
                        ProcessEvent(aResult);
                    }
                }
            });
        }

        private void ProcessEvent(SDL_Event sDL_Event)
        {

            switch (sDL_Event.type)
            {
                case SDL2.SDL.SDL_EventType.SDL_KEYDOWN:

                    break;
                case SDL2.SDL.SDL_EventType.SDL_KEYUP:


                    break;
            }

        }


    }
}
