#include "pch.h"
#include "Array.h"

template<class T>
TArray<T>::TArray()
{
	Data = nullptr;
	Count = Max = 0;
}

template<class T>
int TArray<T>::Num() const
{
	return Count;
}

template<class T>
T& TArray<T>::operator[](int i)
{
	return Data[i];
}

template<class T>
const T& TArray<T>::operator[](int i) const
{
	return Data[i];
};

template<class T>
bool TArray<T>::IsValidIndex(int i) const
{
	return i < Num();
}

int TArray::Add(UObject* NewItem)
{
	Count = Count + 1;
	Max = Max + 1;
	Data = static_cast<UObject**>(malloc(Count * sizeof(UObject*)));
	Data[Count - 1] = NewItem;
	return Count;
}