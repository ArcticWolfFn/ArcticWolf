#pragma once

#include "FortGameState.h"

class AFortGameState_InGame : public AFortGameState
{
public:
	AFortGameState_InGame(InternalUObject*& InternalObject);

	virtual void Setup() override;
};

