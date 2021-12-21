#pragma once

#include "GameModeBase.h"

class AGameMode : public AGameModeBase
{
public:
	AGameMode(UObject* InternalObject);

	void Setup() override; 

	// (Native|Public|BlueprintCallable)
	void StartMatch();

	// (Exec|Native|Public|BlueprintCallable)
	void Say(FString Msg);

private:
	UFunction* Fn_StartMatch;
	UFunction* Fn_Say;

	bool CanExec_StartMatch = false;
	bool CanExec_Say = false;
};

