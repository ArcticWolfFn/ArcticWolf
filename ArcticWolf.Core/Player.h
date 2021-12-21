#pragma once

#include "PlayerController.h"
#include "Finder.h"

class UPlayer : public UObject
{
public:
	UPlayer(ObjectFinder PlayerFinder);

	APlayerController* PlayerController;

	void Setup() override;

private:
	ObjectFinder PlayerFinder;
};

