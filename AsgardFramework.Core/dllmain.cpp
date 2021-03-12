#include "pch.h"
#include "exports.h"
#include "Logger.h"

static EndSceneHook* hook;

HANDLE InitInteraction(frame** queue)
{
	EndSceneHook::queue = queue;

	return EndSceneHook::executionEvent;
}

void RegisterFunctions(func_init_data** functions, int functionsCount)
{
	std::vector<func_init_data> funcsVector;
	for (auto i = 0; i < functionsCount; i++)
	{
		funcsVector.push_back(*functions[i]);
	}
	EndSceneHook::loadFunctions(funcsVector);
}

BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD ul_reason_for_call,
                      LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		hook = new EndSceneHook(reinterpret_cast<func_t>(RegisterFunctions));
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		delete hook;
		break;
	}
	return TRUE;
}
