#pragma once

#include "GameViewportClient.h"

class UEngine : GIObject
{
public:
	UEngine();

	void Setup() override;

	UGameViewportClient GameViewport;

	FortniteGameInstance GameInstance;
};

