#pragma once

#include "GIObject.h"

class AGameStateBase : protected GIObject
{
public:
	// Only used for conversions
	AGameStateBase();
	AGameStateBase(UObject* InternalObject);

	virtual void Setup() override;

	UObject* GetInternalObject();

protected:
	UObject* InternalObject;
};

