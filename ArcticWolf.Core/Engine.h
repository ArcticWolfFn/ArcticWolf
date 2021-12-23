#pragma once

#include "GameViewportClient.h"

class UEngine : GIObject
{
public:
	void Setup() override;

	UGameViewportClient GameViewport = NULL;

	FortniteGameInstance GameInstance;
};

