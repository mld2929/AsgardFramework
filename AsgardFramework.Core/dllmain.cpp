#include "pch.h"
#include "exports.h"
#include "Logger.h"

static std::unique_ptr<EndSceneHook> hook;

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
		hook = std::make_unique<EndSceneHook>(reinterpret_cast<func_t>(RegisterFunctions));
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}
