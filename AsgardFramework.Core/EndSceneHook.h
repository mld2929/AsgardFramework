#pragma once
#include "pch.h"

#include <map>
#include <string>
#include <vector>

using func_t = int(__cdecl*)(...);
using end_scene_t = void(*)();

enum class func_type : int
{
	f_cdecl,
	f_std,
	f_this,
	f_virtual
};

#pragma pack(push, 1)
struct unknown
{
	char bytes[0x397C]{};
	IDirect3DDevice9** device;
};

// E0300 does not allow manipulations with virtual functions
struct device_raw
{
	char bytes[0xA8]{};
	end_scene_t EndScene;
};

struct func_descriptor
{
	func_t func;
	func_type type;
	int argsCount;
};

struct func_init_data
{
	const char8_t* name;
	const func_descriptor desc;
};

struct func_call_data
{
	// c-style utf8 string
	const char8_t* name;
	// array of args
	const int* args;
	int result;
};

struct frame
{
	int count;
	func_call_data** call_data;
};

#pragma pack(pop)


class EndSceneHook
{
	static std::map<std::u8string, func_descriptor, std::less<>> functions;
	static end_scene_t endScene;
public:
	EndSceneHook(func_t registerFunctions);
	~EndSceneHook();
	static HANDLE executionEvent;
	static void loadFunctions(const std::vector<func_init_data>& data);
	static frame** queue;
	static const func_descriptor& get_descriptor(const char8_t* name);
};
