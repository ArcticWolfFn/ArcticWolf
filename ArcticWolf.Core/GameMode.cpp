#include "pch.h"
#include "GameMode.h"

AGameMode::AGameMode(UObject* InternalObject) : AGameModeBase(InternalObject)
{
}

void AGameMode::Setup()
{
	__super::Setup();

	SetPointer(XOR(L"Function /Script/Engine.GameMode:StartMatch"), &Fn_StartMatch, &CanExec_StartMatch);
}

void AGameMode::StartMatch()
{
	if (!CanExec_StartMatch) return;

	ProcessNoParamsEvent(InternalObject, Fn_StartMatch);
}
