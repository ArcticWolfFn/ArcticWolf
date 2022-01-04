#include "pch.h"
#include "FortPlayerController.h"

AFortPlayerController::AFortPlayerController()
{
}

AFortPlayerController::AFortPlayerController(APlayerController* PlayerController)
{
	APlayerController::APlayerController(PlayerController);
	Setup();
}

void AFortPlayerController::Setup()
{
	__super::Setup();

	SetPointer(XOR(L"Function /Script/FortniteGame.FortPlayerController:ServerReadyToStartMatch"), &Fn_ServerReadyToStartMatch, &CanExec_ServerReadyToStartMatch);
}

void AFortPlayerController::ServerReadyToStartMatch()
{
	if (!CanExec_ServerReadyToStartMatch) return;

	ProcessNoParamsEvent(InternalObject, Fn_ServerReadyToStartMatch);
}
