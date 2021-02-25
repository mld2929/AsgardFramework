#include "pch.h"
#define LOG

#include "Hook.h"
#ifdef LOG
#include "Logger.h"
#endif

BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD ul_reason_for_call,
                      LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
#ifdef LOG
		Logger::init();
#endif
		Hook::hookVirtualAlloc();
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		Hook::resetHookVirtualAlloc();
#ifdef LOG
		Logger::release();
#endif
		break;
	default: ;
	}
	return TRUE;
}
