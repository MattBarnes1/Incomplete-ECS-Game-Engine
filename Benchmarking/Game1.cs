

using Engine;
using Engine.Resources;

namespace Benchmarking
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : DoggoEngine
    {

        
        public Game1() : base("Legends of Dragoon")
        {
           // graphics = new GraphicsDeviceManager(this);
           // Content.RootDirectory = "Content";
        }

        public override void SetupGame(World currentWorld, ResourceManager myResourceManager)
        {

        }
    }
}
