using EngineRenderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.Utilities
{
    public class GraphicsSettings
    {
        public GraphicsSettings() { }


        public ANTIALIAS myAliasType { get; set; } = ANTIALIAS.NONE;
        public uint SCREEN_WIDTH { get; set; } = 1024;
        public uint SCREEN_HEIGHT { get; set; } = 720;
        public RendererType myRendererType { get; set; } = RendererType.VULKAN;



    }
}
