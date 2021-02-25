#include "pch.h"
#include "Logger.h"

#include <cstdio>


void Logger::init()
{
	AllocConsole();
	freopen("CONOUT$", "w", stdout);
}

void Logger::release()
{
	FreeConsole();
}
