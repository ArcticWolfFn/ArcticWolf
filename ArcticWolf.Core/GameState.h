#pragma once

#include "GameStateBase.h"

class AGameState : public AGameStateBase
{
public:
	AGameState(InternalUObject*& InternalObject);

	virtual void Setup() override;
};

