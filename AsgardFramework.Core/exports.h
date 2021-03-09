#pragma once
#include "EndSceneHook.h"

extern "C" {
__declspec(dllexport) int InitInteraction(func_init_data** functions, int functionsCount, func_call_data* queue);
}
