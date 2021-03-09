#include "pch.h"
#include "EndSceneHook.h"
#include <algorithm>
#include <map>

#include <iostream>

static unknown** unknown_;
static end_scene_t endScene;

std::map<std::u8string, func_descriptor, std::less<>> EndSceneHook::functions;
func_call_data* EndSceneHook::queue;
HANDLE EndSceneHook::executionEvent;

static int perform_call(const func_descriptor& desc, const int* args)
{
	int* stack_array;
	auto func = desc.func;
	switch (desc.type)
	{
	case func_type::f_cdecl:
	case func_type::f_std:
		stack_array = static_cast<int*>(_alloca(sizeof(int) * desc.argsCount));
		std::copy_n(args, desc.argsCount, stack_array);
		break;
	case func_type::f_virtual:
		func = reinterpret_cast<func_t*>(args[0])[reinterpret_cast<int>(desc.func)];[[fallthrough]];
	case func_type::f_this:
		stack_array = static_cast<int*>(_alloca(sizeof(int) * desc.argsCount - 1));
		std::copy_n(args + 1, desc.argsCount - 1, stack_array);
		__asm {
			mov ecx, args[0]
			}
		break;
	default:
		return 0;
	}
	__asm {
		mov esp, stack_array
		}
	return func();
}

const func_descriptor& EndSceneHook::get_descriptor(const char8_t* name)
{
	return functions[name];
}

static __declspec(naked) void hook()
{
	__asm {
		pushad
		pushfd
		}
	if (WaitForSingleObject(EndSceneHook::executionEvent, 0) == WAIT_OBJECT_0)
	{
		for (auto* data = EndSceneHook::queue; data->name; data++)
		{
			data->result = perform_call(EndSceneHook::get_descriptor(data->name), data->args);
		}
		SetEvent(EndSceneHook::executionEvent);
	}
	__asm {
		popfd
		popad
		jmp endScene
		}
}

EndSceneHook::EndSceneHook()
{
	executionEvent = CreateEventW(nullptr, true, true, nullptr);
	DWORD old;
	unknown_ = reinterpret_cast<unknown**>(0xC5DF88);
	VirtualProtect(unknown_, sizeof(unknown*), PAGE_EXECUTE_READWRITE, &old);
	VirtualProtect(*unknown_, sizeof(unknown), PAGE_EXECUTE_READWRITE, &old);
	auto* raw = reinterpret_cast<device_raw*>((*unknown_)->device);
	VirtualProtect(raw, sizeof(device_raw), PAGE_EXECUTE_READWRITE, &old);
	endScene = raw->EndScene;
	raw->EndScene = hook;
}

EndSceneHook::~EndSceneHook()
{
	reinterpret_cast<device_raw*>((*unknown_)->device)->EndScene = endScene;
	CloseHandle(executionEvent);
}

void EndSceneHook::loadFunctions(const std::vector<func_init_data>& data)
{
	for (const auto& d : data)
	{
		functions[d.name] = d.desc;
	}
}
