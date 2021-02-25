#pragma once

class Logger
{
	Logger() = default;
public:
	static void init();
	static void release();
};
