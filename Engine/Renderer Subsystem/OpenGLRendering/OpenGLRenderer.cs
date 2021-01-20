using Engine.Components;
using Engine.DataTypes;
using EngineRenderer;
using EngineRenderer.Interfaces;
using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Khronos;
using System.Numerics;
using Engine.Low_Level.OpenGL;
using Engine.Renderer.Vulkan.Descriptor_Sets;

namespace Engine.Renderer
{
    public class OpenGLRenderer : RendererBase, IRenderingManager
    {
        float[] vertices = {
    -0.5f, -0.5f, 0.0f, //Bottom-left vertex
     0.5f, -0.5f, 0.0f, //Bottom-right vertex
     0.0f,  0.5f, 0.0f  //Top vertex
}; 
        private static readonly float[] _ArrayPosition = new float[] {
            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f
        };

        public void SetCamera(CameraComponent cameraComponent)
        {
            this.ActiveCamera = cameraComponent;
        }
        /// <summary>
        /// Vertex color array.
        /// </summary>
        private static readonly float[] _ArrayColor = new float[] {
            1.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 1.0f
        };

        private readonly string[] _VertexSourceGL = {
            "#version 150 compatibility\n",
            "uniform mat4 uMVP;\n",
            "in vec2 aPosition;\n",
            "in vec3 aColor;\n",
            "out vec3 vColor;\n",
            "void main() {\n",
            "	gl_Position = uMVP * vec4(aPosition, 0.0, 1.0);\n",
            "	vColor = aColor;\n",
            "}\n",
};
        private VertexArray _VertexArray;

        private readonly string[] _FragmentSourceGL = {
            "#version 150 compatibility\n",
            "in vec3 vColor;\n",
            "void main() {\n",
            "	gl_FragColor = vec4(vColor, 1.0);\n",
            "}\n"
        };
        object Lock = new object();
        public void Draw(TextureComponent myTexture, Rectangle myRectangle)
        {
            lock(Lock)
            {
                Matrix4x4 projection = Matrix4x4.CreateOrthographic(0.0f, 1.0f, 0.0f, 1.0f);
                Matrix4x4 modelview = Matrix4x4.CreateRotationZ(30);

                Gl.UseProgram(ProgramName);

                // Select the program for drawing
                Gl.UseProgram(ProgramName);
                // Set uniform state
                Gl.UniformMatrix4f(LocationMVP, 1, false, projection * modelview);
                // Use the vertex array
                Gl.BindVertexArray(_VertexArray.ArrayName);
                // Draw triangle
                // Note: vertex attributes are streamed from GPU memory
                Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }
        }

        public void DrawWorldSpace(uint ID, ModelComponent myModel, TextureComponent myTexture, TransformComponent myTransform)
        {

        }

        public uint ProgramName { get; private set; }
        public CameraComponent ActiveCamera { get; private set; }

        RendererWindow WindowHandle;
        private int LocationColor;
        private int LocationPosition;
        private int LocationMVP;

        public void InitializeRenderer(RendererWindow WindowHandle, IPreferredGraphicsDeviceFilter myPreferencesForDevice = null)
        {

            Gl.Enable(EnableCap.Multisample);
            byte[] ShaderData = null;
            Program myProgram = new Program(_VertexSourceGL, _FragmentSourceGL);

            int linked;

            Gl.GetProgram(ProgramName, ProgramProperty.LinkStatus, out linked);
            if (linked == 0) throw new Exception("Damn");
            if ((LocationMVP = Gl.GetUniformLocation(ProgramName, "uMVP")) < 0)
                throw new InvalidOperationException("no uniform uMVP");

            // Get attributes locations
            if ((LocationPosition = Gl.GetAttribLocation(ProgramName, "aPosition")) < 0)
                throw new InvalidOperationException("no attribute aPosition");
            if ((LocationColor = Gl.GetAttribLocation(ProgramName, "aColor")) < 0)
                throw new InvalidOperationException("no attribute aColor");
            this.WindowHandle = WindowHandle;
            _VertexArray = new VertexArray(myProgram, _ArrayPosition, _ArrayColor);
            Gl.Viewport(0, 0, WindowHandle.Width, WindowHandle.Height);

            Gl.ClearColor(Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B, Color.AliceBlue.A);
        }

        public void RenderEnd()
        {
            WindowHandle.SwapWindow();


        }

        public void RenderStart()
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);
          
        }

        public ResourceSet CreateDefaultModelResources()
        {
            throw new NotImplementedException();
        }

        public void InitializeRenderer(RendererWindow WindowHandle, IPreferredGraphicsDeviceFilter myPreferencesForDevice = null, bool Deferred = true)
        {
            throw new NotImplementedException();
        }
    }
}
