#pragma once

#include "Array.h"
#include <string>

class FString : public TArray<wchar_t>
{

public:
	FString();
	FString(const wchar_t* other);

	bool IsValid() const;

	const wchar_t* ToWString() const;

	std::string ToString() const;
};