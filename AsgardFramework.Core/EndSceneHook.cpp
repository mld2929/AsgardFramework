#include "pch.h"
#include "EndSceneHook.h"
#include <algorithm>
#include <map>

#include <iostream>

static unknown** unknown_;
end_scene_t EndSceneHook::endScene;

std::map<std::u8string, func_descriptor, std::less<>> EndSceneHook::functions;
frame** EndSceneHook::queue;
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

std::exception getLastErrorException(const char* msg)
{
	return std::exception(msg, GetLastError());
}

static void execute()
{
	for (auto** p_pframe = EndSceneHook::queue; auto* pframe = *p_pframe; p_pframe++)
	{
		auto count = pframe->count;
		for (auto i = 0; i < count; i++)
		{
			auto* data = pframe->call_data[i];
			data->result = perform_call(EndSceneHook::get_descriptor(data->name), data->args);
		}
	}
}

static __declspec(naked) void hook()
{
	__asm {
		pushad
		pushfd
		}
	int code;
	if ((code = WaitForSingleObject(EndSceneHook::executionEvent, 0)) == -1)
		throw getLastErrorException("WaitForSingleObject returned 0xFFFFFFFF");
	if (code != WAIT_OBJECT_0)
	{
		execute();
		SetEvent(EndSceneHook::executionEvent);
	}
	__asm {
		popfd
		popad
		jmp endScene
		}
}

// todo: find EndScene via DirectX
EndSceneHook::EndSceneHook(func_t registerFunctions)
{
	executionEvent = CreateEventW(nullptr, true, true, nullptr);
	unknown_ = reinterpret_cast<unknown**>(0xC5DF88);
	auto** device = (*unknown_)->device;
	endScene = reinterpret_cast<device_raw*>(*device)->EndScene;
	reinterpret_cast<device_raw*>(*device)->EndScene = hook;
	functions[u8"RegisterFunctions"] = func_descriptor{registerFunctions, func_type::f_cdecl, 2};
}

EndSceneHook::~EndSceneHook()
{
	auto** device = (*unknown_)->device;
	reinterpret_cast<device_raw*>(*device)->EndScene = endScene;
	CloseHandle(executionEvent);
}

void EndSceneHook::loadFunctions(const std::vector<func_init_data>& data)
{
	for (const auto& d : data)
	{
		functions[d.name] = d.desc;
	}
}
