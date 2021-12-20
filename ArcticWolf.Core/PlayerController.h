#pragma once

#include "CheatManager.h"
#include "Finder.h"

class APlayerController : GIObject
{
public:
	APlayerController(ObjectFinder* PlayerControllerFinder);

	void Setup() override;

	UCheatManager CheatManager = NULL;

private:
	ObjectFinder* PlayerControllerFinder = nullptr;
};

