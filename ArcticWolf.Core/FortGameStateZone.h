#pragma once

#include "FortGameState_InGame.h"

class AFortGameStateZone : public AFortGameState_InGame
{
public:
	AFortGameStateZone(InternalUObject*& InternalObject);

	virtual void Setup() override;
};

