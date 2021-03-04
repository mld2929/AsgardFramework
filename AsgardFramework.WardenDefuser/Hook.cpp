#include "pch.h"

#include <TlHelp32.h>

#include "Hook.h"

#include <cstdio>
#include <vector>

#include "Logger.h"


#pragma pack(push, 1)

struct origFunc
{
	BYTE garbage[8]{};
	int* jumpTo;
};
#pragma pack(pop)

static int virtualProtectKernelBase = 0;
const void* wardenLoadRet = reinterpret_cast<const void*>(0x008725E0);

void waterwalk()
{
	DWORD old;
	auto* patch = reinterpret_cast<int*>(0x0075E439);
	VirtualProtect(patch, sizeof(int), PAGE_EXECUTE_READWRITE, &old);
	*patch = 0xCF811475;
}


static void __declspec(naked) __cdecl vpHook(const unsigned char* lpAddress, unsigned dwSize, unsigned flNewProtect)
{
	static void* from = nullptr;
	__asm {
		mov eax, [esp]
		mov from, eax
		push ebp
		mov ebp, esp
		}
	printf("Protect 0x%p, called from 0x%p, flNewProtect: [0x%X], size: [0x%X]\n", static_cast<const void*>(lpAddress),
	       from, flNewProtect, dwSize);
	char buffer[9];
	_itoa(flNewProtect, buffer, 16);
	Logger::dump(lpAddress, dwSize, buffer);
	__asm {
		pop ebp
		jmp dword ptr[virtualProtectKernelBase]
		}
}

void Hook::hookVirtualProtect()
{
	auto* hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, GetCurrentProcessId());
	MODULEENTRY32W me;
	ZeroMemory(&me, sizeof(me));
	me.dwSize = sizeof(me);
	auto eq = false;
	while (Module32NextW(hSnapshot, &me) && !(eq = _wcsicmp(L"kernel32.dll", me.szModule) == 0))
	{
	}
	if (!eq)
		printf("Kernel32.dll not found\n");
	else
	{
		DWORD old;
		auto* k32_vp = reinterpret_cast<origFunc*>(GetProcAddress(me.hModule, "VirtualProtect"));
		VirtualProtect(k32_vp->jumpTo, sizeof(int), PAGE_EXECUTE_READWRITE, &old);
		virtualProtectKernelBase = *k32_vp->jumpTo;
		*k32_vp->jumpTo = reinterpret_cast<int>(vpHook);
	}
	CloseHandle(hSnapshot);
}

void Hook::resetHookVirtualProtect()
{
}
