using System;

namespace Vulkan
{
    internal class NativePointer
	{
		internal NativeReference Reference { get; private set; }
		internal IntPtr Handle { get; private set; }

		internal NativePointer (NativeReference reference, IntPtr pointer)
		{
			Reference = reference;
			Handle = pointer;
		}

		internal NativePointer (NativeReference reference)
		{
			Reference = reference;
			Handle = reference.Handle;
		}

		internal void Release ()
		{
			Reference = null;
			Handle = IntPtr.Zero;
		}
	}
}
