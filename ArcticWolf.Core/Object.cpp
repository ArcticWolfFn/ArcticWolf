#include "pch.h"
#include "Object.h"

void UObject::ProcessEvent(void* fn, void* parms)
{
	auto vtable = *reinterpret_cast<void***>(this);
	auto processEventFn = static_cast<void(*)(void*, void*, void*)>(vtable[0x44]);
	processEventFn(this, fn, parms);
}

bool UObject::IsA(UClass* cmp) const
{
	if (this->Class == cmp)
	{
		return true;
	}
	return false;
}

FName UObject::GetFName() const
{
	return *reinterpret_cast<const FName*>(this + 0x18);
}