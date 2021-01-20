using EngineRenderer;
using EngineRenderer.Interfaces;
using EngineRenderer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Engine.Components;
using Engine.Renderer;
using OpenGL;
using Khronos;
using static SDL2.SDL;
using Engine.Utilities;
using Engine.LowLevelInterface;

namespace EngineRenderer
{
    /// <summary>
    /// This class initializes the Window our Renderer will use via SDL's Create Window class. It acts as a wrapper for the underlying unmanaged memory pointer for the window.
    /// </summary>
    public class RendererWindow
    {
        public IntPtr WindowHandle { get; private set; }
        public uint WindowID { get; private set; }
        public IntPtr context { get; private set; }



        public IRenderingManager myEngineToUse { get; private set; }


        public unsafe RendererWindow(int WindowX, int WindowY, String Name, RendererType Flags, uint Width, uint Height)// int X, int Y, int Width, int Height, SDL2.SDL_WINDOW_FLAGS Flags = SDL2.SDL_WINDOW_FLAGS.SDL_WINDOW_VULKAN)
        {

            switch (Flags)
            {
                case RendererType.OPENGL:
                    SDL2.SDL.SDL_InitSubSystem((uint)SDL_INIT_FLAGS.SDL_INIT_VIDEO);
                    SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_ES);
                    SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 2);
                    SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 1);
                    WindowHandle = SDL2.SDL.SDL_CreateWindow(Name, WindowX, WindowY, (int)Width, (int)Height, SDL2.SDL_WINDOW_FLAGS.SDL_WINDOW_OPENGL);
                    if (WindowHandle == IntPtr.Zero) // Failed to Create Window
                    {
                        throw new RENDER_WINDOW_CREATION_FAILED("Failed to create window that is compatible with OpenGL.");
                    }
                    int Index = SDL2.SDL.SDL_GetWindowDisplayIndex(WindowHandle);
                    WindowID = SDL2.SDL.SDL_GetWindowID(WindowHandle);
                    var Res = SDL2.SDL.SDL_GL_GetCurrentWindow();

                    SDL_GLcontext context = (SDL_GLcontext)SDL2.SDL.SDL_GL_CreateContext(WindowHandle);                
                    int Result = SDL2.SDL.SDL_GL_MakeCurrent(WindowHandle, (IntPtr)context);
                    SDL_CreateRenderer(WindowHandle, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);


                    Gl.Initialize();
                    
                    if (Result != 0)
                        throw new RENDER_WINDOW_CREATION_FAILED("Failed to properly initialize OpenGL");

                    // Gl.BindAPI();
                    myEngineToUse = new OpenGLRenderer();
                    break;
                case RendererType.VULKAN:
                    SDL2.SDL.SDL_InitSubSystem((uint)SDL_INIT_FLAGS.SDL_INIT_VIDEO);
                    WindowHandle = SDL2.SDL.SDL_CreateWindow(Name, WindowX, WindowY, (int)Width, (int)Height, SDL2.SDL_WINDOW_FLAGS.SDL_WINDOW_VULKAN);
                    if (WindowHandle == IntPtr.Zero) // Failed to Create Window
                    {
                        throw new RENDER_WINDOW_CREATION_FAILED("Failed to create window that is compatible with Vulkan.");
                    }
                    break;
            }

        }

        public void SwapWindow()
        {
            SDL2.SDL.SDL_GL_SwapWindow(WindowHandle);
        }


    }
}
