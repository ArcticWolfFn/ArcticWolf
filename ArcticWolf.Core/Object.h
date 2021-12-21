#pragma once
#include "NameTypes.h"
#include "UnrealType.h"
#include "GIObject.h"
class UClass;
//class GIObject;

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

