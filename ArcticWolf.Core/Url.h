#pragma once
#include <algorithm>
#include <string>
#include <sstream>
#include <string_view>

struct Uri
{
private:
	typedef std::string_view::const_iterator iterator_t;

public:
	std::string_view Protocol, Host, Port, Path, QueryString;

	static Uri Parse(const std::string_view& uri);

	static std::string CreateUri(std::string_view Protocol, std::string_view Host, std::string_view Port, std::string_view Path, std::string_view QueryString);

private:
	static constexpr std::string_view make_string_view(const std::string_view& base, iterator_t first, iterator_t last);
};
