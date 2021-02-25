#pragma once
class Hook
{
	Hook() = default;
	static void hook(void*, void*, int, int, int);
	static int hookend();
public:
	static void hookVirtualAlloc();
	static void resetHookVirtualAlloc();
};
