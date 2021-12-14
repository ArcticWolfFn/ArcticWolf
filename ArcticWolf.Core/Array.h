#pragma once

#include "Object.h"

template <class T>
class TArray
{
	friend class FString;

public:

	T* Data;
	int32_t Count;
	int32_t Max;

	TArray();

	int Num() const;

	T& operator[](int i);

	const T& operator[](int i) const;

	bool IsValidIndex(int i) const;

	int Add(UObject* NewItem);
};
