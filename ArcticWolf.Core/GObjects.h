#pragma once

#include "Object.h"

struct FUObjectItem
{
	InternalUObject* Object;
	DWORD Flags;
	DWORD ClusterIndex;
	DWORD SerialNumber;
	DWORD SerialNumber2;
};

struct PreFUObjectItem
{
	FUObjectItem* Objects[10];
};

class GObjects
{
public:
	PreFUObjectItem* ObjectArray;
	BYTE unknown1[8];
	int32_t MaxElements;
	int32_t NumElements;

	void NumChunks(int* start, int* end) const;

	InternalUObject* GetByIndex(int32_t index) const;
};

