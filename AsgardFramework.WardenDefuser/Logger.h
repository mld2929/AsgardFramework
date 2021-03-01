#pragma once
#include <memory>
#include <string>

class Logger
{
	Logger() = default;
	static std::unique_ptr<std::ofstream> out;
public:
	static void init();
	static void release();
	static void dump(const unsigned char* start, unsigned size, const std::string& description);
	static void dump(const unsigned char* start, unsigned size, const char* szDescription);
};
