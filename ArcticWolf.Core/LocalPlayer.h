#pragma once

#include "Player.h"

class ULocalPlayer : public UPlayer
{
public:
	ULocalPlayer(ObjectFinder* LocalPlayerFinder);

	virtual void Setup() override;
};

