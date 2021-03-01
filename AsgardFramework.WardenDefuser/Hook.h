#pragma once
class Hook
{
	Hook() = default;
public:
	static void hookVirtualProtect();
	static void resetHookVirtualProtect();
};
