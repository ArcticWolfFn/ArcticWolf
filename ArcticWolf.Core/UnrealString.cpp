#include "pch.h"
#include "UnrealString.h"
#include <locale>

FString::FString()
{

}

FString::FString(const wchar_t* other)
{
	Max = Count = *other ? std::wcslen(other) + 1 : 0;

	if (Count)
	{
		Data = const_cast<wchar_t*>(other);
	}
}

bool FString::IsValid() const
{
	return Data != nullptr;
}

const wchar_t* FString::ToWString() const
{
	if (Util::IsBadReadPtr(Data))
	{
		return L"";
	}
	return Data;
}

std::string FString::ToString() const
{
	auto length = std::wcslen(Data);

	std::string str(length, '\0');

	std::use_facet<std::ctype<wchar_t>>(std::locale()).narrow(Data, Data + length, '?', &str[0]);

	return str;
}
