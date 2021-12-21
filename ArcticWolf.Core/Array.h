#pragma once

#include <cstdlib>

class UObject;

template <class T>
class TArray
{
	friend class FString;

public:

	T* Data = nullptr;
	int32_t Count = 0;
	int32_t Max = 0;

	TArray()
	{
		Data = nullptr;
		Count = Max = 0;
	};

	int Num() const
	{
		return Count;
	};

	T& operator[](int i)
	{
		return Data[i];
	};

	const T& operator[](int i) const
	{
		return Data[i];
	};

	bool IsValidIndex(int i) const
	{
		return i < Num();
	}

	// Function does not work
	/*int Add(UObject* NewItem)
	{
		Count = Count + 1;
		Max = Max + 1;
		Data = static_cast<UObject**>(malloc(Count * sizeof(UObject*)));
		Data[Count - 1] = NewItem;
		return Count;
	}*/
};
