#pragma once

#include "LocalPlayer.h"

class UGameInstance : GIObject
{
public:

	ULocalPlayer* LocalPlayers[1];

	void Setup() override;
};

