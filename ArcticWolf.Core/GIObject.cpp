#include "pch.h"
#include "GIObject.h"
#include "ue4.h"

GIObject::GIObject()
{
}

void GIObject::Setup()
{
}

void GIObject::ProcessNoParamsEvent(void* obj, void* fn)
{
	ProcessEvent(obj, fn, &emptyParams);
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