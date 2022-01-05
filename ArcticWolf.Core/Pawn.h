#pragma once

#include "Actor.h"

class APawn : public AActor
{
public:
	APawn();
	APawn(InternalUObject*& object);

	virtual void Setup() override;
};

