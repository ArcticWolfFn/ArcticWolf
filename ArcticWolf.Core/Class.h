#pragma once
#include "Field.h"

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

struct UFunction : UStruct
{
	int32_t FunctionFlags;
	int16_t RepOffset;
	int8_t NumParms;
	char unknown1[1];
	int16_t ParmsSize;
	int16_t ReturnValueOffset;
	int16_t RPCId;
	int16_t RPCResponseId;
	class UProperty* FirstPropertyToInit;
	UFunction* EventGraphFunction;
	int32_t EventGraphCallOffset;
	void* Func;
};