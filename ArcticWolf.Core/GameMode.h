#pragma once

#include "GameModeBase.h"

class AGameMode : public AGameModeBase
{
public:
	AGameMode(AGameModeBase* GameModeBase);
	AGameMode(InternalUObject* InternalObject);

	void Setup() override; 

	// (Native|Public|BlueprintCallable)
	void StartMatch();

	// (Exec|Native|Public|BlueprintCallable)
	void Say(FString Msg);

private:
	UFunction* Fn_StartMatch = nullptr;
	UFunction* Fn_Say = nullptr;

	bool CanExec_StartMatch = false;
	bool CanExec_Say = false;
};

