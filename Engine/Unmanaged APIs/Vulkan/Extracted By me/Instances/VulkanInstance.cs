using EngineRenderer;
using EngineRenderer.Exceptions.VULKAN;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Vulkan.Interop;

namespace Vulkan
{
	public partial class VulkanInstance : IDisposable, IMarshalling
	{
		private NativeMethods.vkCreateDebugUtilsCallbackExt vkCreateDebugUtilsCallbackExt;
		NativeMethods.vkCreateDebugReportCallbackEXT vkCreateDebugReportCallbackEXT;
		NativeMethods.vkDestroyDebugReportCallbackEXT vkDestroyDebugReportCallbackEXT;
		NativeMethods.vkDebugReportMessageEXT vkDebugReportMessageEXT;
		private NativeMethods.DebugUtilsMessengerCallback vkDebugUtilsMessage;

		Delegate GetMethod(string name, Type type)
		{
			var funcPtr = GetProcAddr(name);

			if (funcPtr == IntPtr.Zero)
				return null;

			return Marshal.GetDelegateForFunctionPointer(funcPtr, type);
		}

		internal IntPtr GetWindowHandle()
		{
			return this.SDLWindowHandle;
		}

		public VulkanInstance(InstanceCreateInfo CreateInfo, AllocationCallbacks Allocator = null)
		{
			Result result;

			unsafe {
				fixed (IntPtr* ptrInstance = &m) {
					result = Interop.NativeMethods.vkCreateInstance(CreateInfo.m, Allocator != null ? Allocator.m : null, ptrInstance);
				}
			}

			if (result != Result.Success)
				throw new ResultException(result);

		}

		~VulkanInstance()
		{
			Debug.WriteLine("VULKAN INSTANCE DELETED! ===========================================================================");
			int i = 0;
		}

		public VulkanInstance() : this(new InstanceCreateInfo())
		{
		}
		InstanceCreateInfo myInfoCreatedWith;
		IntPtr SDLWindowHandle;
		public unsafe VulkanInstance(string ApplicationName, IntPtr SDLWindowHandle, AllocationCallbacks Allocator = null)
		{
			this.SDLWindowHandle = SDLWindowHandle;
			uint Count;
			IntPtr myPtr;
			List<String> myStrings = new List<string>();
			SDL2.SDL.SDL_Vulkan_GetInstanceExtensions(SDLWindowHandle, &Count, null);
			if (Count == 0) throw new VULKAN_NO_HARDWARE_SUPPORTED_DEVICES();
			SDL2.SDL.SDL_Vulkan_GetInstanceExtensions(SDLWindowHandle, &Count, out myPtr);
			for (int i = 0; i < Count; i++)
			{
				void* Test = myPtr.ToPointer();
				string aString = Marshal.PtrToStringAnsi(myPtr);
				myStrings.Add(aString);
				myPtr = IntPtr.Add(myPtr, myStrings[i].Length + 1);
			}
#if DEBUG
			myStrings.Add("VK_EXT_debug_report");
			//myStrings.Add("VK_EXT_debug_utils");

#endif
			List<Interop.LayerProperties> Layers = new List<Interop.LayerProperties>();
			unsafe
			{
				String myValues;
				uint myCount;
				Interop.LayerProperties* myProperties;
				Interop.NativeMethods.vkEnumerateInstanceLayerProperties(&myCount, null);
				int SizeofBuffer = Marshal.SizeOf(typeof(Interop.LayerProperties));
				IntPtr myStartingBlock = Marshal.AllocHGlobal((int)myCount * SizeofBuffer);
				IntPtr Memory = myStartingBlock;

				Interop.NativeMethods.vkEnumerateInstanceLayerProperties(&myCount, (Interop.LayerProperties*)Memory.ToPointer());
				int* Positions = ((int*)myStartingBlock.ToPointer());
				int Last = 0;
				for (int i = 0; i < myCount; i++)
				{
					Interop.LayerProperties myProperties2 = new Interop.LayerProperties();
					int* PropertiesToCopy = (int*)&myProperties2;
					int q = 0;
					for (q = Last; q < Last + (SizeofBuffer / Marshal.SizeOf(typeof(int))); q++)
					{
						PropertiesToCopy[q - Last] = Positions[q];
					}
					Last = q;
					Layers.Add(myProperties2);
				}
				Marshal.FreeHGlobal(myStartingBlock);
			}
#if DEBUG
			List<String> LayerNames = new List<string>();
			foreach (Interop.LayerProperties a in Layers)
			{
				byte* AName = a.LayerName;
				string A = Marshal.PtrToStringAnsi(new IntPtr(AName));
				Debug.WriteLine(A);
#if RENDERDOCENABLED
				if(A.Contains("RENDERDOC") || A.Contains("VK_LAYER_LUNARG_standard_validation"))
				{
					LayerNames.Add(A);
				}
#else
				if (A.Contains("VK_LAYER_LUNARG_standard_validation") || A.Contains("VK_LAYER_KHRONOS_validation"))
				{
					LayerNames.Add(A);
				}

#endif
			}
#endif



				myInfoCreatedWith = new InstanceCreateInfo()
			{
#if DEBUG
				 EnabledLayerCount = (uint)LayerNames.Count,
				 EnabledLayerNames = LayerNames.ToArray(),
#endif
				EnabledExtensionNames = myStrings.ToArray(),
				ApplicationInfo = new ApplicationInfo()
				{
					ApiVersion = Vulkan.Version.Make(1, 0, 0),
					ApplicationName = ApplicationName,
					ApplicationVersion = Vulkan.Version.Make(0, 0, 1)
				}
			}; ;


			Result result;

			unsafe
			{
				fixed (IntPtr* ptrInstance = &m)
				{
					result = Interop.NativeMethods.vkCreateInstance(myInfoCreatedWith.m, Allocator != null ? Allocator.m : null, ptrInstance);
				}
			}

			if (result != Result.Success)
				throw new ResultException(result);

			vkCreateDebugUtilsCallbackExt = (NativeMethods.vkCreateDebugUtilsCallbackExt)GetMethod("vkCreateDebugUtilsMessengerEXT", typeof(NativeMethods.vkCreateDebugUtilsCallbackExt));
			//TODO:vkDestroyDebugUtilsCallbackExt = (NativeMethods.DebugUtilsMessengerCallback)GetMethod("vkDestroyDebugUtilsMessengerEXT", typeof(NativeMethods.DebugUtilsMessengerCallback));

			vkCreateDebugReportCallbackEXT = (NativeMethods.vkCreateDebugReportCallbackEXT)GetMethod("vkCreateDebugReportCallbackEXT", typeof(NativeMethods.vkCreateDebugReportCallbackEXT));
			vkDestroyDebugReportCallbackEXT = (NativeMethods.vkDestroyDebugReportCallbackEXT)GetMethod("vkDestroyDebugReportCallbackEXT", typeof(NativeMethods.vkDestroyDebugReportCallbackEXT));
			vkDebugReportMessageEXT = (NativeMethods.vkDebugReportMessageEXT)GetMethod("vkDebugReportMessageEXT", typeof(NativeMethods.vkDebugReportMessageEXT));
			vkDebugUtilsMessage = (NativeMethods.DebugUtilsMessengerCallback)GetMethod("vkDebugUtilsMessengerEXT", typeof(NativeMethods.vkCreateDebugUtilsCallbackExt));

#if DEBUG
			EnableDebug(DebugCallback);
			//EnableValidation(ValidationCallback);
#endif


		}

		private Bool32 ValidationCallback(DebugUtilsMessageSeverityFlag severity, DebugUtilsMessageTypeFlag myType, DebugUtilsMessengerCallbackData myData, IntPtr myUserData)
		{
			//if(severity == DebugUtilsMessageSeverityFlag.VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT)
				Debug.WriteLine(Marshal.PtrToStringAnsi(myData.pMessage));

		//	Debug.WriteLine($"{severity}: {Marshal.PtrToStringAnsi(myData.)}");
			return true;
		}

		private Bool32 DebugCallback(DebugReportFlagsExt flags, DebugReportObjectTypeExt objectType, ulong objectHandle, IntPtr location, int messageCode, IntPtr layerPrefix, IntPtr message, IntPtr userData)
		{
			if (flags != DebugReportFlagsExt.Information)
				Debug.WriteLine($"{flags}: {Marshal.PtrToStringAnsi(message)}");
			return true;
		}

		public void Dispose ()
		{
			if (debugCallback != null && vkDestroyDebugReportCallbackEXT != null) {
				DestroyDebugReportCallbackEXT (debugCallback);
				debugCallback = null;
			}
			if (m != IntPtr.Zero) {
				Destroy ();
				m = IntPtr.Zero;
			}
		}
		//Debug
		public delegate Bool32 DebugReportCallback (DebugReportFlagsExt flags, DebugReportObjectTypeExt objectType, ulong objectHandle, IntPtr location, int messageCode, IntPtr layerPrefix, IntPtr message, IntPtr userData);
		public delegate Bool32 DebugUtilsCallback(DebugUtilsMessageSeverityFlag severity, DebugUtilsMessageTypeFlag myType, DebugUtilsMessengerCallbackData myData, IntPtr myUserData);
		static DebugUtilsCallback myUtilsCallback;
		DebugReportCallbackCreateInfo debugCreateInfo;
		static DebugReportCallbackExt debugCallback;
		public void EnableDebug (DebugReportCallback d, DebugReportFlagsExt flags = DebugReportFlagsExt.Debug | DebugReportFlagsExt.Error | DebugReportFlagsExt.Information | DebugReportFlagsExt.PerformanceWarning | DebugReportFlagsExt.Warning)
		{
			if (vkCreateDebugReportCallbackEXT == null)
				throw new InvalidOperationException ("vkCreateDebugReportCallbackEXT is not available, possibly you might be missing VK_EXT_debug_report extension. Try to enable it when creating the Instance.");

			debugCreateInfo = new DebugReportCallbackCreateInfo () {
				Flags = flags,				
				PfnCallback = Marshal.GetFunctionPointerForDelegate (d)
			};

			if (debugCallback != null)
				DestroyDebugReportCallbackEXT (debugCallback);
			debugCallback = CreateDebugReportCallbackEXT (debugCreateInfo);
		}

		public void EnableValidation(DebugUtilsCallback d, DebugUtilsMessageTypeFlag flags = DebugUtilsMessageTypeFlag.VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | DebugUtilsMessageTypeFlag.VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT | DebugUtilsMessageTypeFlag.VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT)
		{
			if (vkCreateDebugReportCallbackEXT == null)
				throw new InvalidOperationException("vkCreateDebugReportCallbackEXT is not available, possibly you might be missing VK_EXT_debug_report extension. Try to enable it when creating the Instance.");

			debugUtilsCreateInfo = new DebugUtilsMessengerCreateInfoEXT()
			{
				Flags = 0, 
				messageSeverity = DebugUtilsMessageSeverityFlag.VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | DebugUtilsMessageSeverityFlag.VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT | DebugUtilsMessageSeverityFlag.VK_DEBUG_UTILS_MESSAGE_SEVERITY_INFO_BIT_EXT,
				messageType = flags,
				sType = StructureType.DEBUGUTILSMESSENGERCREATEINFOEXT,
				pfnUserCallback = Marshal.GetFunctionPointerForDelegate(d),
			};

			
			unsafe
			{
				fixed (DebugUtilsMessengerCreateInfoEXT *Debug = &debugUtilsCreateInfo)
				fixed(UInt64 *Callback = &aaaa)
				{
					 vkCreateDebugUtilsCallbackExt.Invoke(this.m, Debug, null, Callback);
				}
			}
		}
		IntPtr DebugUtil;//Todo: make a class for this like the others.
		UInt64 aaaa;
		public DebugReportCallbackExt CreateDebugReportCallbackEXT(DebugReportCallbackCreateInfo pCreateInfo, AllocationCallbacks pAllocator = null)
		{
			Result result;
			DebugReportCallbackExt pCallback;
			unsafe
			{
				pCallback = new DebugReportCallbackExt();

				fixed (UInt64* ptrpCallback = &pCallback.m)
				{
					result = vkCreateDebugReportCallbackEXT(this.m, pCreateInfo != null ? pCreateInfo.m : (Interop.DebugReportCallbackCreateInfoExt*)default(IntPtr), pAllocator != null ? pAllocator.m : null, ptrpCallback);
				}
				if (result != Result.Success)
					throw new ResultException(result);

				return pCallback;
			}
		}

		


		public void DestroyDebugReportCallbackEXT(DebugReportCallbackExt callback, AllocationCallbacks pAllocator = null)
		{
			unsafe
			{
				vkDestroyDebugReportCallbackEXT(this.m, callback != null ? callback.m : default(UInt64), pAllocator != null ? pAllocator.m : null);
			}
		}

		public void DebugReportMessageEXT(DebugReportFlagsExt flags, DebugReportObjectTypeExt objectType, UInt64 @object, UIntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage)
		{
			unsafe
			{
				vkDebugReportMessageEXT(this.m, flags, objectType, @object, location, messageCode, pLayerPrefix, pMessage);
			}
		}

		//Debug
		internal IntPtr m;
		private DebugUtilsMessengerCreateInfoEXT debugUtilsCreateInfo;

		IntPtr IMarshalling.Handle
		{
			get
			{
				return m;
			}
		}

		public void Destroy(AllocationCallbacks pAllocator = null)
		{
			unsafe
			{
				Interop.NativeMethods.vkDestroyInstance(this.m, pAllocator != null ? pAllocator.m : null);
			}
		}

		public VulkanPhysicalDevice[] EnumeratePhysicalDevices()
		{
			Result result;
			unsafe
			{
				UInt32 pPhysicalDeviceCount;
				result = Interop.NativeMethods.vkEnumeratePhysicalDevices(this.m, &pPhysicalDeviceCount, null);
				if (result != Result.Success)
					throw new ResultException(result);
				if (pPhysicalDeviceCount <= 0)
					throw new VULKAN_NO_HARDWARE_SUPPORTED_DEVICES();

				int size = Marshal.SizeOf(typeof(IntPtr));
				var refpPhysicalDevices = new NativeReference((int)(size * pPhysicalDeviceCount));
				var ptrpPhysicalDevices = refpPhysicalDevices.Handle;
				result = Interop.NativeMethods.vkEnumeratePhysicalDevices(this.m, &pPhysicalDeviceCount, (IntPtr*)ptrpPhysicalDevices);
				if (result != Result.Success)
					throw new ResultException(result);

				if (pPhysicalDeviceCount <= 0)
					return null;
				var arr = new VulkanPhysicalDevice[pPhysicalDeviceCount];
				for (int i = 0; i < pPhysicalDeviceCount; i++)
				{
					arr[i] = new VulkanPhysicalDevice(((IntPtr*)ptrpPhysicalDevices)[i]);
				}

				return arr;
			}
		}

		public IntPtr GetProcAddr(string pName)
		{
			unsafe
			{
				return Interop.NativeMethods.vkGetInstanceProcAddr(this.m, pName);
			}
		}


		
		public SurfaceKHR CreateDisplayPlaneSurfaceKHR(DisplaySurfaceCreateInfoKHR pCreateInfo, AllocationCallbacks pAllocator = null)
		{
			Result result;
			SurfaceKHR pSurface;
			unsafe
			{
				pSurface = new SurfaceKHR();

				fixed (UInt64* ptrpSurface = &pSurface.m)
				{
					result = Interop.NativeMethods.vkCreateDisplayPlaneSurfaceKHR(this.m, pCreateInfo != null ? pCreateInfo.m : (Interop.DisplaySurfaceCreateInfoKHR*)default(IntPtr), pAllocator != null ? pAllocator.m : null, ptrpSurface);
				}
				if (result != Result.Success)
					throw new ResultException(result);

				return pSurface;
			}
		}

		public SurfaceKHR CreateViSurfaceNN(ViSurfaceCreateInfoNn pCreateInfo, AllocationCallbacks pAllocator = null)
		{
			Result result;
			SurfaceKHR pSurface;
			unsafe
			{
				pSurface = new SurfaceKHR();

				fixed (UInt64* ptrpSurface = &pSurface.m)
				{
					result = Interop.NativeMethods.vkCreateViSurfaceNN(this.m, pCreateInfo != null ? pCreateInfo.m : (Interop.ViSurfaceCreateInfoNn*)default(IntPtr), pAllocator != null ? pAllocator.m : null, ptrpSurface);
				}
				if (result != Result.Success)
					throw new ResultException(result);

				return pSurface;
			}
		}

		public SurfaceKHR CreateIOSSurfaceMVK(IOSSurfaceCreateInfoMvk pCreateInfo, AllocationCallbacks pAllocator = null)
		{
			Result result;
			SurfaceKHR pSurface;
			unsafe
			{
				pSurface = new SurfaceKHR();

				fixed (UInt64* ptrpSurface = &pSurface.m)
				{
					result = Interop.NativeMethods.vkCreateIOSSurfaceMVK(this.m, pCreateInfo != null ? pCreateInfo.m : (Interop.IOSSurfaceCreateInfoMvk*)default(IntPtr), pAllocator != null ? pAllocator.m : null, ptrpSurface);
				}
				if (result != Result.Success)
					throw new ResultException(result);

				return pSurface;
			}
		}

		public SurfaceKHR CreateMacOSSurfaceMVK(MacOSSurfaceCreateInfoMvk pCreateInfo, AllocationCallbacks pAllocator = null)
		{
			Result result;
			SurfaceKHR pSurface;
			unsafe
			{
				pSurface = new SurfaceKHR();

				fixed (UInt64* ptrpSurface = &pSurface.m)
				{
					result = Interop.NativeMethods.vkCreateMacOSSurfaceMVK(this.m, pCreateInfo != null ? pCreateInfo.m : (Interop.MacOSSurfaceCreateInfoMvk*)default(IntPtr), pAllocator != null ? pAllocator.m : null, ptrpSurface);
				}
				if (result != Result.Success)
					throw new ResultException(result);

				return pSurface;
			}
		}
	}
}
