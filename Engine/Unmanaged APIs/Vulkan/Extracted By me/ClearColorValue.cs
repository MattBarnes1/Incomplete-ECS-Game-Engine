using Engine.Renderer;
using System;

namespace Vulkan
{
    unsafe public class ClearColorValue : MarshalledObject
	{

		public float[] Float32
		{
			get
			{
				var arr = new float[4];
				for (int i = 0; i < 4; i++)
					arr[i] = m->Float32[i];
				return arr;
			}

			set
			{
				if (value.Length > 4)
					throw new Exception("array too long");
				for (int i = 0; i < value.Length; i++)
					m->Float32[i] = value[i];
				for (int i = value.Length; i < 4; i++)
					m->Float32[i] = 0;
			}
		}

		public Int32[] Int32
		{
			get
			{
				var arr = new Int32[4];
				for (int i = 0; i < 4; i++)
					arr[i] = m->Int32[i];
				return arr;
			}

			set
			{
				if (value.Length > 4)
					throw new Exception("array too long");
				for (int i = 0; i < value.Length; i++)
					m->Int32[i] = value[i];
				for (int i = value.Length; i < 4; i++)
					m->Int32[i] = 0;
			}
		}

		public UInt32[] Uint32
		{
			get
			{
				var arr = new UInt32[4];
				for (int i = 0; i < 4; i++)
					arr[i] = m->Uint32[i];
				return arr;
			}

			set
			{
				if (value.Length > 4)
					throw new Exception("array too long");
				for (int i = 0; i < value.Length; i++)
					m->Uint32[i] = value[i];
				for (int i = value.Length; i < 4; i++)
					m->Uint32[i] = 0;
			}
		}
		internal Interop.ClearColorValue* m
		{

			get
			{
				return (Interop.ClearColorValue*)native.Handle;
			}
		}

		public ClearColorValue()
		{
			native = Interop.Structure.Allocate(typeof(Interop.ClearColorValue));
		}

		internal ClearColorValue(NativePointer pointer)
		{
			native = pointer;
		}

		public ClearColorValue(Color aColor) : this(new uint[] { aColor.R, aColor.G, aColor.B, aColor.A })
		{
		}

		public ClearColorValue (float [] floatArray) : this ()
		{
			Float32 = floatArray;
		}

		public ClearColorValue (int [] intArray) : this ()
		{
			Int32 = intArray;
		}

		public ClearColorValue (uint [] uintArray) : this ()
		{
			Uint32 = uintArray;
		}
	}
}
