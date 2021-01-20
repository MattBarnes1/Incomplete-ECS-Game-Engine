using System.Numerics;
using System.Runtime.InteropServices;

namespace EngineRenderer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MVPUniformBuffer
    {     
        public Matrix4x4 Model;
        public Matrix4x4 View;
        public Matrix4x4 Proj;
    };
}