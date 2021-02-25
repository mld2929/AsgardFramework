#include "pch.h"

#include <TlHelp32.h>

#include "Hook.h"

#include <cstdio>


#pragma pack(push, 1)

struct jmp
{
	BYTE opcode[2]{0xFF, 0x25};
	int* to;

	explicit jmp(int* dest) : to(dest)
	{
	}
};

struct call
{
	BYTE data[7]{0xBB, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xD3};

	explicit call(const int to)
	{
		reinterpret_cast<int&>(data[1]) = to;
	}
};

struct origData
{
	BYTE data[6]{};
};

struct origFunc
{
	origData data;
	jmp jumpTo;
};

struct modified
{
	call toHook;
	call toImpl;
	jmp toHookend;

	modified(const call& hook, const call& toVA, const jmp& hookend) : toHook(hook), toImpl(toVA), toHookend(hookend)
	{
	}
};
#pragma pack(pop)

static int buffer = 0;

void Hook::hookVirtualAlloc()
{
	auto* hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, GetCurrentProcessId());
	MODULEENTRY32W me;
	ZeroMemory(&me, sizeof(me));
	me.dwSize = sizeof(me);
	auto eq = false;
	while (Module32NextW(hSnapshot, &me) && !(eq = _wcsicmp(L"kernel32.dll", me.szModule) == 0)) {}
	if (!eq)
		printf("Kernel32.dll not found\n");
	else
	{
		DWORD old;
		auto* k32_va = reinterpret_cast<origFunc*>(reinterpret_cast<int>(GetProcAddress(me.hModule, "VirtualAlloc")));
		VirtualProtect(k32_va, sizeof(modified), PAGE_EXECUTE_READWRITE, &old);
		const auto to = *k32_va->jumpTo.to;
		printf("Hook will be set at 0x%X\n", reinterpret_cast<int>(k32_va));
		buffer = reinterpret_cast<int>(hookend);
		*reinterpret_cast<modified*>(k32_va) = modified(call(reinterpret_cast<int>(hook)), call(to), jmp(&buffer));
	}
	CloseHandle(hSnapshot);
}

void Hook::resetHookVirtualAlloc()
{
}


void __cdecl Hook::hook(void* from, void* lpAddress, int size, int allocType, int protection)
{
	printf("Requested allocation at 0x%p, size 0x%X, called from 0x%p\n flAllocationType: [0x%X], flProtect: [0x%X]\n",
	       lpAddress, size, from, allocType, protection);
	// stack patch
	__asm{
		mov edx, esp
		mov esp, ebp
		add esp, 8
		mov ecx, 16
		l_loop:
		mov eax, [esp + ecx]
		xchg[esp], eax
		mov [esp + ecx], eax
		sub ecx, 4
		cmp ecx, 0
		jne l_loop
		mov esp, edx
		}
}

int __cdecl Hook::hookend()
{
	int addr;
	__asm mov addr, eax
	printf("Allocated at 0x%X\n", addr);
	return addr;
}
