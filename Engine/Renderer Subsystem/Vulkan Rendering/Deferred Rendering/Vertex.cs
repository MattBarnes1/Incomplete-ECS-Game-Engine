using System.Numerics;

namespace Engine.Renderer.Vulkan_Rendering.RenderPasses
{
   public struct Vertex
    {
        public Vector3 pos;
        public Vector2 uv;
        public Vector3 col;
        public Vector3 normal;
        public Vector3 tangent;

        public Vertex(Vector3 pos, Vector2 uv, Vector3 col, Vector3 normal, Vector3 tangent)
        {
            this.pos = pos;
            this.uv = uv;
            this.col = col;
            this.normal = normal;
            this.tangent = tangent;
        }
    };
}
