#pragma once

#include "GameState.h"

class APlayspaceGameState : public AGameState
{
public:
	APlayspaceGameState(InternalUObject*& InternalObject);

	virtual void Setup() override;
};

