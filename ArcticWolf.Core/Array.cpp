#include "pch.h"
#include "Array.h"

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