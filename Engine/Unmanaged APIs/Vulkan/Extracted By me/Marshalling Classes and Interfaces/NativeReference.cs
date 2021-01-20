using System;
using System.Runtime.InteropServices;

namespace Vulkan
{
    internal class NativeReference : IDisposable
	{
		internal IntPtr Handle { get; private set; }

		internal NativeReference (int size, bool zero = false)
		{
			Handle = Marshal.AllocHGlobal (size);
			if (NativeMemoryDebug.Enabled) {
				lock (NativeMemoryDebug.Allocations) {
					NativeMemoryDebug.Allocations [Handle] = size;
					NativeMemoryDebug.AllocatedSize += size;
				}
			}
			if (!zero)
				return;
			unsafe
			{
				byte* bptr = (byte*)Handle;
				for (int i = 0; i < size; i++)
					bptr [i] = 0;
			}
		}

		public void Dispose ()
		{
			if (Handle != IntPtr.Zero) {
				if (NativeMemoryDebug.Enabled) {
					lock (NativeMemoryDebug.Allocations) {
						NativeMemoryDebug.AllocatedSize -= NativeMemoryDebug.Allocations [Handle];
						if (NativeMemoryDebug.Allocations.ContainsKey (Handle))
							NativeMemoryDebug.Allocations.Remove (Handle);
						else
							NativeMemoryDebug.Report ("unknown handle found: {0}", Handle);
					}
				}
				Marshal.FreeHGlobal (Handle);
			}
			Handle = IntPtr.Zero;
		}

		~NativeReference ()
		{
			Dispose ();
		}
	}
}
