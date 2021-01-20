﻿using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Low_Level.OpenGL
{
	public class VertexArray : IDisposable
	{
		public VertexArray(Program program, float[] positions, float[] colors)
		{
			if (program == null)
				throw new ArgumentNullException(nameof(program));

			// Allocate buffers referenced by this vertex array
			_BufferPosition = new Buffer(positions);
			_BufferColor = new Buffer(colors);

			// Generate VAO name
			ArrayName = Gl.GenVertexArray();
			// First bind create the VAO
			Gl.BindVertexArray(ArrayName);

			// Set position attribute

			// Select the buffer object
			Gl.BindBuffer(BufferTarget.ArrayBuffer, _BufferPosition.BufferName);
			// Format the vertex information: 2 floats from the current buffer
			Gl.VertexAttribPointer((uint)program.LocationPosition, 2, VertexAttribType.Float, false, 0, IntPtr.Zero);
			// Enable attribute
			Gl.EnableVertexAttribArray((uint)program.LocationPosition);

			// As above, but for color attribute
			Gl.BindBuffer(BufferTarget.ArrayBuffer, _BufferColor.BufferName);
			Gl.VertexAttribPointer((uint)program.LocationColor, 3, VertexAttribType.Float, false, 0, IntPtr.Zero);
			Gl.EnableVertexAttribArray((uint)program.LocationColor);
		}

		public readonly uint ArrayName;

		private readonly Buffer _BufferPosition;

		private readonly Buffer _BufferColor;

		public void Dispose()
		{
			Gl.DeleteVertexArrays(ArrayName);

			_BufferPosition.Dispose();
			_BufferColor.Dispose();
		}
	}

}
