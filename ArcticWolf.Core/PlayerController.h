#pragma once

#include "CheatManager.h"
#include "Finder.h"

class APlayerController : protected GIObject
{
public:
	APlayerController();
	APlayerController(ObjectFinder* PlayerControllerFinder);

	void Setup() override;

	// (Exec|Native|Public)
	void SwitchLevel(FString URL);

	UCheatManager CheatManager = NULL;

protected:
	UObject* InternalObject = nullptr;

private:
	ObjectFinder* PlayerControllerFinder;

	UFunction* Fn_SwitchLevel = nullptr;

	bool CanExec_SwitchLevel = false;
};

