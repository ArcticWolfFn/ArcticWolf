#pragma once

template <class T>
class TArray
{
	friend class FString;

public:

	T* Data = nullptr;
	int32_t Count = 0;
	int32_t Max = 0;

	int Num() const;

	T& operator[](int i);

	const T& operator[](int i) const;

	bool IsValidIndex(int i) const;

	int Add(T* NewItem);
};
