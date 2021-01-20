using System;

namespace Vulkan
{
    public class MarshalledObject : IDisposable, IMarshalling
	{
		internal NativePointer native;

		public IntPtr Handle {
			get {
				return native.Handle;
			}
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}




		public virtual void Dispose (bool disposing)
		{
			if(disposing)
			{
				native.Release();
				native = null;
			}
		}

		~MarshalledObject ()
		{
			Dispose (false);
		}
	}
}
