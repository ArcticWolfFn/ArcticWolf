#include "pch.h"
#include "GIObject.h"
#include "ue4.h"

void GIObject::Setup()
{
}

template<typename T>
inline void GIObject::SetPointer(wchar_t const* objectToFind, T* objectToSet, bool* success)
{
	T* obj = UE4::FindObject<T*>(objectToFind);

	if (Util::IsBadReadPtr(obj))
	{
		PLOGE.printf("%s is nullptr", objectToFind);
		if (success != nullptr) 
		{
			success = false;
		}
		return;
	}

	objectToSet = obj;
	if (success != nullptr)
	{
		success = true;
	}
}