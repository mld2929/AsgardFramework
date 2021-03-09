#include "pch.h"
#include "Logger.h"

#include <fstream>
#include <iomanip>
#include <sstream>
#include <vector>

std::unique_ptr<std::ofstream> Logger::out;

void Logger::init()
{
	AllocConsole();
	freopen("CONOUT$", "w", stdout);
	out = std::make_unique<std::ofstream>("dump.txt");
}

void Logger::release()
{
	FreeConsole();
	out->flush();
	out->close();
}

void Logger::dump(const unsigned char* start, unsigned size, const std::string& description)
{
	*out << "DUMP [0x" << std::hex << reinterpret_cast<int>(start) << " -- 0x" << std::hex << reinterpret_cast<int>(
		start + size) << "] (" << description << "):" << std::endl;
	std::vector<unsigned char> data(start, start + size);
	std::stringstream str_stream;
	for (const auto b : data)
		str_stream << std::setfill('0') << std::setw(2) << std::hex << static_cast<int>(b);
	*out << str_stream.str() << std::endl;
}

void Logger::dump(const unsigned char* start, unsigned size, const char* szDescription)
{
	auto desc = std::string(szDescription);
	dump(start, size, desc);
}
