#pragma once
#include "NameTypes.h"
#include "UnrealType.h"

class UClass;

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

