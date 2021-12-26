#pragma once

#include "GameViewportClient.h"

class UEngine : GIObject
{
public:
	void Setup() override;

	UGameViewportClient* GameViewport = nullptr;

	FortniteGameInstance GameInstance;
};

