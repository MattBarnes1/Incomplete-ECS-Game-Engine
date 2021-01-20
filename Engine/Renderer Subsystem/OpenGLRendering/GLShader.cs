using OpenGL;
using System;
using System.Text;

namespace Engine.Renderer
{
    internal class GLShader : IDisposable
    {
        private ShaderType vertexShader;
        private String[] shaderData;

        public uint ShaderName { get; }

        public GLShader(ShaderType vertexShader, String[] shaderData)
        {
            this.vertexShader = vertexShader;
            this.shaderData = shaderData;
            ShaderName = Gl.CreateShader(vertexShader);
            Gl.ShaderSource(ShaderName, shaderData);
            Gl.CompileShader(ShaderName);
            int compiled;
            Gl.GetShader(ShaderName, ShaderParameterName.CompileStatus, out compiled);
            if (compiled != 0)
                return;
            else
            {
                const int logMaxLength = 1024;
                StringBuilder infolog = new StringBuilder(logMaxLength+1);
                int infologLength;
                Gl.GetShaderInfoLog(ShaderName, logMaxLength, out infologLength, infolog);

                throw new InvalidOperationException($"unable to compile shader: {infolog}");
            }
        }

        public void Dispose()
        {
        }
    }
}