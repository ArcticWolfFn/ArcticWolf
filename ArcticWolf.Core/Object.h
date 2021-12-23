#pragma once
#include "NameTypes.h"
#include "UnrealType.h"
#include "GIObject.h"
class UClass;
class InternalUClass;
//class GIObject;

struct InternalUObject
{
	PVOID VTableObject;
	DWORD ObjectFlags;
	DWORD InternalIndex;
	InternalUClass* Class;
	FName NamePrivate;
	InternalUObject* Outer;

	void ProcessEvent(void* fn, void* parms)
	{
		auto vtable = *reinterpret_cast<void***>(this);
		auto processEventFn = static_cast<void(*)(void*, void*, void*)>(vtable[0x44]);
		processEventFn(this, fn, parms);
	}

	bool IsA(InternalUClass* cmp) const
	{
		if (this->Class == cmp)
		{
			return true;
		}
		return false;
	}

	FName GetFName() const
	{
		return *reinterpret_cast<const FName*>(this + 0x18);
	}
};

struct InternalUField : InternalUObject
{
	InternalUField* Next;
	void* padding_01;
	void* padding_02;
};

struct InternalUStruct : InternalUField
{
	InternalUStruct* SuperStruct;
	InternalUField* Children;
	FField* ChildProperties;
	int32_t PropertiesSize;
	int32_t MinAlignment;
	TArray<uint8_t> Script;
	FProperty* PropertyLink;
	FProperty* RefLink;
	FProperty* DestructorLink;
	FProperty* PostConstructLink;
};

struct InternalUClass : InternalUStruct
{
};

class UObject : public GIObject
{
public:
	PVOID VTableObject;
	DWORD ObjectFlags;
	DWORD InternalIndex;
	UClass* Class;
	FName NamePrivate;
	UObject* Outer;

	virtual void Setup() override;
};

struct UField : public UObject
{
public:
	UField* Next;
	void* padding_01;
	void* padding_02;
};

class UStruct : public UField
{
public:
	UStruct* SuperStruct;
	UField* Children;
	FField* ChildProperties;
	int32_t PropertiesSize;
	int32_t MinAlignment;
	TArray<uint8_t> Script;
	FProperty* PropertyLink;
	FProperty* RefLink;
	FProperty* DestructorLink;
	FProperty* PostConstructLink;
};

class UClass : public UStruct
{
};

