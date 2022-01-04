#include "pch.h"
#include "FortPlayerController.h"

UFunction* AFortPlayerController::Fn_ServerReadyToStartMatch = nullptr;
bool AFortPlayerController::CanExec_ServerReadyToStartMatch = false;

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

	if (Util::IsBadReadPtr(Fn_ServerReadyToStartMatch))
	{
		PLOGE << "Fn_ServerReadyToStartMatch is nullptr";
		return;
	}

	if (Util::IsBadReadPtr(GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject))
	{
		PLOGE << "InternalObject is nullptr";
		return;
	}

	ProcessNoParamsEvent(GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject, Fn_ServerReadyToStartMatch);
}
