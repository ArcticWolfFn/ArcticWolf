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

	UCheatManager* CheatManager = nullptr;

	ObjectFinder* PlayerControllerFinder = nullptr;

protected:
	InternalUObject*& InternalObject;

private:
	UFunction* Fn_SwitchLevel = nullptr;

	bool CanExec_SwitchLevel = false;
};

