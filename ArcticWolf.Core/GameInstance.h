#pragma once

#include "LocalPlayer.h"

class UGameInstance : GIObject
{
public:

	std::vector<ULocalPlayer> LocalPlayers;

	virtual void Setup() override;
};

