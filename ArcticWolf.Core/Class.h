#pragma once
#include "Field.h"
#include "Array.h"
#include "UnrealType.h"

class UClass : public UStruct
{
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

struct UField : public UObject
{
	UField* Next;
	void* padding_01;
	void* padding_02;
};
