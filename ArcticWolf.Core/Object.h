#pragma once
#include "Class.h"
#include "NameTypes.h"
class UObject
{
public:
	PVOID VTableObject;
	DWORD ObjectFlags;
	DWORD InternalIndex;
	UClass* Class;
	FName NamePrivate;
	UObject* Outer;

	void ProcessEvent(void* fn, void* parms);

	bool IsA(UClass* cmp) const;

	FName GetFName() const;
};

