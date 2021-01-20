﻿using Engine.Renderer;
using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Low_Level.OpenGL
{
	public class Program : IDisposable
	{
		public Program(string[] vertexSource, string[] fragmentSource)
		{
			// Create vertex and frament shaders
			// Note: they can be disposed after linking to program; resources are freed when deleting the program
			using (GLShader vObject = new GLShader(ShaderType.VertexShader, vertexSource))
			using (GLShader fObject = new GLShader(ShaderType.FragmentShader, fragmentSource))
			{
				// Create program
				ProgramName = Gl.CreateProgram();
				// Attach shaders
				Gl.AttachShader(ProgramName, vObject.ShaderName);
				Gl.AttachShader(ProgramName, fObject.ShaderName);
				// Link program
				Gl.LinkProgram(ProgramName);

				// Check linkage status
				int linked;

				Gl.GetProgram(ProgramName, ProgramProperty.LinkStatus, out linked);

				if (linked == 0)
				{
					const int logMaxLength = 1024;

					StringBuilder infolog = new StringBuilder(logMaxLength);
					int infologLength;

					Gl.GetProgramInfoLog(ProgramName, 1024, out infologLength, infolog);

					throw new InvalidOperationException($"unable to link program: {infolog}");
				}

				// Get uniform locations
				if ((LocationMVP = Gl.GetUniformLocation(ProgramName, "uMVP")) < 0)
					throw new InvalidOperationException("no uniform uMVP");

				// Get attributes locations
				if ((LocationPosition = Gl.GetAttribLocation(ProgramName, "aPosition")) < 0)
					throw new InvalidOperationException("no attribute aPosition");
				if ((LocationColor = Gl.GetAttribLocation(ProgramName, "aColor")) < 0)
					throw new InvalidOperationException("no attribute aColor");
			}
		}

		public readonly uint ProgramName;

		public readonly int LocationMVP;

		public readonly int LocationPosition;

		public readonly int LocationColor;

		public void Dispose()
		{
			Gl.DeleteProgram(ProgramName);
		}
	}
}
