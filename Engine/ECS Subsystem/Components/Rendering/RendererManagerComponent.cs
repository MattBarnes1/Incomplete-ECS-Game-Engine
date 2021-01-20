using Engine.ECS.Components.Base;
using Engine.Utilities;
using EngineRenderer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Components
{
    public class RendererManagerComponent : SingletonComponent<RendererManagerComponent>
    {
        public IRenderingManager myRenderEngine { get; private set; }

        public RendererManagerComponent()
        {
        }

        public void Initialize(IRenderingManager myRenderer)
        {
            myRenderEngine = myRenderer;
        }
    }
}
