#pragma once

#include "Player.h"

class ULocalPlayer : public UPlayer
{
public:
	ULocalPlayer(ObjectFinder LocalPlayerFinder);

	void Setup() override;
};

