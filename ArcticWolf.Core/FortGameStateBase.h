#pragma once

#include "PlayspaceGameState.h"

class AFortGameStateBase : public APlayspaceGameState
{
public:
	AFortGameStateBase(InternalUObject*& InternalObject);

	virtual void Setup() override;
};

