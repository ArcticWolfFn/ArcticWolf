#pragma once

#include "CheatManager.h"
#include "Finder.h"

class APlayerController : protected GIObject
{
public:
	APlayerController();
	APlayerController(ObjectFinder* PlayerControllerFinder);
	APlayerController(APlayerController* PlayerController);

	void Setup() override;

	// (Exec|Native|Public)
	void SwitchLevel(FString URL);

	// (Exec|Native|Public)
	void LocalTravel(FString URL);

	UCheatManager* CheatManager = nullptr;

	ObjectFinder* PlayerControllerFinder = nullptr;

	InternalUObject*& InternalObject;

private:
	static UFunction* Fn_SwitchLevel;
	static UFunction* Fn_LocalTravel;

	static bool CanExec_SwitchLevel;
	static bool CanExec_LocalTravel;
};

