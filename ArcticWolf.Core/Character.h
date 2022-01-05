#pragma once

#include "Pawn.h"

class ACharacter : public APawn
{
public:
	ACharacter();
	ACharacter(InternalUObject* InternalObject);
	virtual void Setup() override;

protected:
	InternalUObject* InternalObject = nullptr;
};

