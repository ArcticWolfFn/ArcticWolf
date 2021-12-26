#pragma once

#include "FortGameStateBase.h"

class AFortGameState : public AFortGameStateBase
{
public:
	AFortGameState(InternalUObject*& InternalObject);

	virtual void Setup() override;
};

