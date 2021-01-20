using System;

namespace Vulkan
{
    public interface INonDispatchableHandleMarshalling
	{
		UInt64 Handle { get; }
	}
}
