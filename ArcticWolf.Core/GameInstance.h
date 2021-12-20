#pragma once

#include "LocalPlayer.h"

class UGameInstance : GIObject
{
public:

	TArray<ULocalPlayer> LocalPlayers;

	void Setup() override;
};

