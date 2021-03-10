#pragma once
#include "EndSceneHook.h"

extern "C" {
__declspec(dllexport) HANDLE InitInteraction(func_call_data** queue);
__declspec(dllexport) void RegisterFunctions(func_init_data** functions, int functionsCount);
}
