#pragma once

#include "CheatManager.h"
#include "Finder.h"

class APlayerController : GIObject
{
public:
	APlayerController(ObjectFinder* PlayerControllerFinder);

	void Setup() override;

	// (Exec|Native|Public)
	void SwitchLevel(FString URL);

	UCheatManager CheatManager = NULL;

private:
	ObjectFinder* PlayerControllerFinder = nullptr;
	UObject* InternalPlayerController = nullptr;

	UFunction* Fn_SwitchLevel = nullptr;

	bool CanExec_SwitchLevel = false;
};

