
using Engine.Managers;
using System;
using Engine.Resources;
using EngineRenderer;
using Engine.LowLevelInterface;
using Vulkan;
using static SDL2.SDL;
using Engine.ECS.Components.Base;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Engine.Utilities;
using System.Xml.Serialization;
using System.IO;
using Engine.Components;
using Engine.Initializers;
using Engine.ECS_Subsystem.Components;
using Engine.ECS.Components.Input_Components;//TODO: Fix Namespace Naming
using Engine.ECS_Subsystem.Components.Audio_Components;
using Engine.Engine_Settings;

namespace Engine
{
    /// <summary>
    /// This class sets up the engine's subsystems then runs the main loop.
    /// </summary>
    public abstract class DoggoEngine
    {
        ResourceManager myResourceManager;
        WorldManager myWorldManager;
        SDL_EventFilter myFilterBase;
        public DoggoEngine(String GameName)
        {
            //Initialize SDL without setting up Subsystems
            SDL2.SDL.SDL_Init(0);

            #region Graphics And Compute Interface Loading
            //Get our available graphics Settings
            GraphicsSettings mySettings = EngineSettings.LoadSettingsFile<GraphicsSettings>();//new GraphicsSettings("GraphicsSettings.xml");

            //Load the RendererWindow
            RendererWindow myRenderWindow = new RendererWindow(0, 0, GameName, mySettings.myRendererType, mySettings.SCREEN_WIDTH, mySettings.SCREEN_HEIGHT);

            //Gets our renderer singleton component
            RendererManagerComponent RendererComponentObject = RendererManagerComponent.Get();

            //Gets our Compute singleton component
            ComputeManagerComponent ComputeComponentObject = ComputeManagerComponent.Get();

            //Load proper initializers based on RendererType
            switch (mySettings.myRendererType)
            {
                case RendererType.OPENGL:
                    throw new NotImplementedException();
                    break;
                case RendererType.VULKAN:
                    VulkanInitializer myInitializerClass = new VulkanInitializer(GameName, myRenderWindow, mySettings);
                    RendererComponentObject.Initialize(myInitializerClass.RenderingManager);
                    ComputeComponentObject.Initialize(myInitializerClass.ComputeManager);
                    break;
            }
            #endregion

            //Gets the InputManager's Singleton Component
            InputManagerComponent myManager = InputManagerComponent.Get();

            //Gets the AudioManager's Singleton Component
            AudioManagerComponent myAudioManager = AudioManagerComponent.Get();

            //Prints out all currently unwritten tests that should've been done before actually writing the code.
#if DEBUG
            var Assembly2 = Assembly.GetEntryAssembly();
            List<Type> myTYpes = new List<Type>();
            myTYpes.AddRange(Assembly2.GetTypes());
            myTYpes.AddRange(Assembly.GetExecutingAssembly().GetTypes());
            Debug.WriteLine("/////////////////////////////////////////////////////////////////////////");
            Debug.WriteLine("Unwritten Tests: ");
            foreach (Type a in myTYpes)
            {
                var myAttributes = a.GetCustomAttributes<TestReminderAttribute>() as TestReminderAttribute[];
                foreach(TestReminderAttribute A in myAttributes)
                {
                    A.PrintTestInformational();
                }
            }
            Debug.WriteLine("/////////////////////////////////////////////////////////////////////////");
#endif

            //Creates the world manager with all subsystems inside of it ready for use.
            myWorldManager = new WorldManager();

            //Passes the WorldManager to the developer
            SetupGame(myWorldManager, WorldManager.myCurrentResourceManager);

            //Makes sure that the developer set the active world so that it doesn't throw a cryptic error down the line which requires them to contact me.
            if (myWorldManager.ActiveWorld == null) throw new Exception("An active world wasn't created in SetupGame!");
        }

        public abstract void SetupGame(WorldManager currentWorld, ResourceManager myResourceManager);

        static bool QuitLoop { get; set; } = false;

        public void Run()
        {
            UInt32 SDLStart = SDL2.SDL.SDL_GetTicks();
            UInt32 LastValue = SDL2.SDL.SDL_GetTicks();

            GameTimeUpdateComponent Value = SingletonComponent<GameTimeUpdateComponent>.Get();
            while (!QuitLoop)
            {
                SDLStart = SDL2.SDL.SDL_GetTicks();
                Value.UpdateTime((SDLStart - LastValue) / 1000f);
                SDL2.SDL.SDL_PumpEvents();
                myWorldManager.Update();
                LastValue = SDL2.SDL.SDL_GetTicks();               
            }

        }



    }
}
