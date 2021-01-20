using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineRenderer.Exceptions
{

    [Serializable]
    public class RENDER_WINDOW_CREATION_FAILED : Exception
    {
        public RENDER_WINDOW_CREATION_FAILED(String myError) : base("Fatal Error: " + myError) { }
        public RENDER_WINDOW_CREATION_FAILED() : base("Fatal Error: Render Window Failed to Create!") { }

    }
    [Serializable]
    public class RENDER_WINDOW_NO_VALID_RENDERER : Exception
    {
        public RENDER_WINDOW_NO_VALID_RENDERER() : base("Fatal Error: Render Window couldn't find a valid renderer!") { }

    }
}
