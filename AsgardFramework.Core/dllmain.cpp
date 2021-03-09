#include "pch.h"
#include "exports.h"
#include "Logger.h"

static const EndSceneHook hook;

int InitInteraction(func_init_data** functions, int functionsCount, func_call_data* queue)
{
	EndSceneHook::queue = queue;
	std::vector<func_init_data> funcsVector;
	for (auto i = 0; i < functionsCount; i++)
	{
		funcsVector.push_back(*functions[i]);
	}
	EndSceneHook::loadFunctions(funcsVector);
	return reinterpret_cast<int>(EndSceneHook::executionEvent);
}

BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD ul_reason_for_call,
                      LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		Logger::init();
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		Logger::release();
		break;
	}
	return TRUE;
}
