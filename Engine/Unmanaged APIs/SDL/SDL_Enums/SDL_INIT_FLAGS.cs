using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.LowLevelInterface
{

	[Flags]
	public enum SDL_INIT_FLAGS : uint
	{
		SDL_INIT_TIMER = 0x00000001u,
		SDL_INIT_AUDIO = 0x00000010u,
		SDL_INIT_VIDEO = 0x00000020u,
		SDL_INIT_JOYSTICK = 0x00000200u,
		SDL_INIT_HAPTIC = 0x00001000u,
		SDL_INIT_GAMECONTROLLER = 0x00002000u,
		SDL_INIT_EVENTS = 0x00004000u,
		SDL_INIT_EVERYTHING = SDL_INIT_TIMER | SDL_INIT_AUDIO | SDL_INIT_VIDEO |
		SDL_INIT_JOYSTICK | SDL_INIT_HAPTIC |
		SDL_INIT_GAMECONTROLLER
	}
}
