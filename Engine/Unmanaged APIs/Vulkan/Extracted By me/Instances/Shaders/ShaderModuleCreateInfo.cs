using System;
using System.Runtime.InteropServices;

namespace Vulkan
{

	unsafe public partial class ShaderModuleCreateInfo
	{
		public byte [] CodeBytes {
			set {
				/* todo free allocated memory when already set */
				if (value == null) {
					m->CodeSize = UIntPtr.Zero;
					m->Code = IntPtr.Zero;
					return;
				}
				m->CodeSize = (UIntPtr)value.Length;
				m->Code = Marshal.AllocHGlobal (value.Length);
				Marshal.Copy (value, 0, m->Code, value.Length);
			}
		}
	}
}
