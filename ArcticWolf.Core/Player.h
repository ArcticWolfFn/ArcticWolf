#pragma once

#include "PlayerController.h"
#include "Finder.h"

class UPlayer : public UObject
{
public:
	UPlayer(ObjectFinder* PlayerFinder);

	virtual void Setup() override;
	APlayerController* GetPlayerController();

protected:
	APlayerController* PlayerController = nullptr;

private:
	ObjectFinder* PlayerFinder = nullptr;
};

