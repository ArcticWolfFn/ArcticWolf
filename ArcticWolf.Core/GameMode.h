#pragma once

#include "GameModeBase.h"

class AGameMode : public AGameModeBase
{
public:
	AGameMode(UObject* InternalObject);

	void Setup() override; 

	// (Native|Public|BlueprintCallable)
	void StartMatch();

private:
	UFunction* Fn_StartMatch;

	bool CanExec_StartMatch;
};

