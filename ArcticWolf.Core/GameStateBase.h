#pragma once

#include "GIObject.h"

class AGameStateBase : protected GIObject
{
public:
	AGameStateBase();
	AGameStateBase(InternalUObject*& InternalObject);

	virtual void Setup() override;

	InternalUObject*& GetInternalObjectRef();
	InternalUObject* GetInternalObject();

protected:
	InternalUObject*& InternalObject;
};

