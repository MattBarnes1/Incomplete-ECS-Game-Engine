using Engine.Components;
using Engine.Data_Types;
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
using static SDL2.SDL;

namespace Engine.Default_Systems
{
    [SystemWriter(typeof(MouseInputComponent))]
    public class MouseInputSystem : SingletonComponentUpdateSystem<MouseInputComponent>
    {
        SDL_EventFilter myFilter;
        ConcurrentQueue<SDL2.SDL.SDL_Event> myEvent = new ConcurrentQueue<SDL.SDL_Event>();
        public MouseInputSystem(World MyBase) : base(MyBase)
        {

            DoggoEngine.RegisterEventFilter(MouseInputFilter);
            /*myFilter = new SDL_EventFilter(MouseInputFilter);
            SDL2.SDL.SDL_SetEventFilter(myFilter, new IntPtr());*/


        }

        public int MouseInputFilter(IntPtr userdata, IntPtr sdlevent)
        {

          var Result = (SDL2.SDL.SDL_Event)Marshal.PtrToStructure(sdlevent, typeof(SDL2.SDL.SDL_Event));
           switch(Result.type)
            {
                    case SDL2.SDL.SDL_EventType.SDL_MOUSEMOTION:
                        myEvent.Enqueue(Result);
                        return 0;
                    break;
                    case SDL2.SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        myEvent.Enqueue(Result);
                        return 0;
                    break;
                    case SDL2.SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                        myEvent.Enqueue(Result);
                        return 0;
                    break;
                    case SDL2.SDL.SDL_EventType.SDL_MOUSEWHEEL:
                        myEvent.Enqueue(Result);
                        return 0;
                    break;
                     default:
                        return 1;
                     break;
            }
        }
        public override void Postprocess()
        {
           
        }

        public override void Preprocess()
        {

        }

        protected override void Process(ThreadController myController)
        {
            myController.AddDelegate(delegate (object obj)
            {
                ClearPressedState();
                while (myEvent.Count != 0)
                {
                    if (myEvent.TryDequeue(out var aResult))
                    {
                        switch (aResult.type)
                        {
                            case SDL2.SDL.SDL_EventType.SDL_MOUSEMOTION:
                                UpdateMouseMovement(aResult);
                                break;
                            case SDL2.SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                                UpdateMouseButtonState(aResult);
                                break;
                            case SDL2.SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                                UpdateMouseButtonState(aResult);
                                break;
                            case SDL2.SDL.SDL_EventType.SDL_MOUSEWHEEL:
                                UpdateMouseWheelChange(aResult);

                                break;
                        }
                    }
                }
            });            
        }

        private void ClearPressedState()
        {
            if (GetSingleton().LeftButtonState == ButtonState.Clicked)
                GetSingleton().LeftButtonState = ButtonState.Released;
            if (GetSingleton().RightButtonState == ButtonState.Clicked)
                GetSingleton().RightButtonState = ButtonState.Released;
        }

        private void UpdateMouseButtonState(SDL_Event aResult)
        {
            switch((uint)aResult.button.button)
            {
                case SDL_BUTTON_LEFT:
                    SetButtonState(ref GetSingleton().LeftButtonState, aResult.button.state);
                    SetButtonClickState(ref GetSingleton().LeftButtonState, aResult.button.clicks);
                    break;
                case SDL_BUTTON_RIGHT:
                    SetButtonState(ref GetSingleton().RightButtonState, aResult.button.state);
                    SetButtonClickState(ref GetSingleton().RightButtonState, aResult.button.clicks);
                    break;
                case SDL_BUTTON_MIDDLE:
                    SetButtonState(ref GetSingleton().MiddleButtonState, aResult.button.state);
                    SetButtonClickState(ref GetSingleton().MiddleButtonState, aResult.button.clicks);
                    break;
            }
        }

        private void SetButtonClickState(ref ButtonState leftButtonState, byte clicks)
        {
            if (clicks == 1)
                leftButtonState = ButtonState.Clicked;
            if (clicks == 2)
                leftButtonState = ButtonState.DoubleClicked;
        }

        private void SetButtonState(ref ButtonState leftButtonState, byte state)
        {
            if (state == SDL_PRESSED)
                leftButtonState = ButtonState.Pressed;
            else
                leftButtonState = ButtonState.Released;
        }

        private void UpdateMouseWheelChange(SDL_Event aResult)
        {
        }

        private void UpdateMouseMovement(SDL_Event aResult)
        {
            GetSingleton().MousePosition = new System.Numerics.Vector3(aResult.motion.x, aResult.motion.y, 0);
        }
    }
}
