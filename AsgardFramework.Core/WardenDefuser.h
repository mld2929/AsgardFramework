#pragma once
class WardenDefuser
{
	WardenDefuser() = default;
public:
	static void hookVirtualProtect();
	static void resetHookVirtualProtect();
};
