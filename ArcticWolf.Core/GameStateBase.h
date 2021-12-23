#pragma once

#include "GIObject.h"

class AGameStateBase : protected GIObject
{
public:
	// Only used for conversions
	AGameStateBase();
	AGameStateBase(InternalUObject* InternalObject);

	virtual void Setup() override;

	InternalUObject* GetInternalObject();

protected:
	InternalUObject* InternalObject = nullptr;
};

