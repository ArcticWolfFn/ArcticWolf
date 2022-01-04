#include "pch.h"
#include "FortPlayerController.h"

UFunction* AFortPlayerController::Fn_ServerReadyToStartMatch = nullptr;
UFunction* AFortPlayerController::Fn_ServerSetClientHasFinishedLoading = nullptr;
UFunction* AFortPlayerController::Fn_OnRep_bHasServerFinishedLoading = nullptr;

bool AFortPlayerController::CanExec_ServerReadyToStartMatch = false;
bool AFortPlayerController::CanExec_ServerSetClientHasFinishedLoading = false;
bool AFortPlayerController::CanExec_OnRep_bHasServerFinishedLoading = false;

int32_t AFortPlayerController::Offset_bHasServerFinishedLoading = 0;

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
	SetPointer(XOR(L"Function /Script/FortniteGame.FortPlayerController:ServerSetClientHasFinishedLoading"), &Fn_ServerSetClientHasFinishedLoading, &CanExec_ServerSetClientHasFinishedLoading);
	SetPointer(XOR(L"Function /Script/FortniteGame.FortPlayerController:OnRep_bHasServerFinishedLoading"), &Fn_OnRep_bHasServerFinishedLoading, &CanExec_OnRep_bHasServerFinishedLoading);

	if (Offset_bHasServerFinishedLoading == 0) 
	{
		Offset_bHasServerFinishedLoading = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortGameStateAthena"), XOR(L"GamePhase"));
	}

	bHasServerFinishedLoading = reinterpret_cast<bool*>(reinterpret_cast<uintptr_t>(InternalObject) + Offset_bHasServerFinishedLoading);
}

void AFortPlayerController::ServerReadyToStartMatch()
{
	if (!CanExec_ServerReadyToStartMatch) return;

	if (Util::IsBadReadPtr(Fn_ServerReadyToStartMatch))
	{
		PLOGE << "Fn_ServerReadyToStartMatch is nullptr";
		return;
	}

	InternalObject = GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject;
	if (Util::IsBadReadPtr(InternalObject))
	{
		PLOGE << "InternalObject is nullptr";
		return;
	}

	ProcessNoParamsEvent(InternalObject, Fn_ServerReadyToStartMatch);
}

void AFortPlayerController::ServerSetClientHasFinishedLoading(bool bInHasFinishedLoading)
{
	if (!CanExec_ServerSetClientHasFinishedLoading) return;

	struct Params {
		bool bInHasFinishedLoading;
	};

	auto params = Params();
	params.bInHasFinishedLoading = bInHasFinishedLoading;

	if (Util::IsBadReadPtr(Fn_ServerSetClientHasFinishedLoading))
	{
		PLOGE << "Fn_ServerReadyToStartMatch is nullptr";
		return;
	}

	InternalObject = GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject;
	if (Util::IsBadReadPtr(InternalObject))
	{
		PLOGE << "InternalObject is nullptr";
		return;
	}

	ProcessEvent(InternalObject, Fn_ServerSetClientHasFinishedLoading, &params);
}

void AFortPlayerController::OnRep_bHasServerFinishedLoading()
{
	InternalObject = GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject;
	if (Util::IsBadReadPtr(InternalObject))
	{
		PLOGE << "InternalObject is nullptr";
		return;
	}
	ProcessNoParamsEvent(InternalObject, Fn_OnRep_bHasServerFinishedLoading);
}

void AFortPlayerController::SetbHasServerFinishedLoading(bool bHasServerFinishedLoading)
{
	InternalObject = GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject;

	if (Offset_bHasServerFinishedLoading == 0)
	{
		PLOGE << "Invalid Offset";
		return;
	}

	if (InternalObject == 0)
	{
		PLOGE << "Invalid InternalObject";
		return;
	}

	if (Util::IsBadReadPtr(this->bHasServerFinishedLoading))
	{
		this->bHasServerFinishedLoading = reinterpret_cast<bool*>(reinterpret_cast<uintptr_t>(InternalObject) + Offset_bHasServerFinishedLoading);
	}

	if (Util::IsBadReadPtr(this->bHasServerFinishedLoading))
	{
		PLOGE << "Invalid pointer bHasServerFinishedLoading";
		return;
	}

	*this->bHasServerFinishedLoading = bHasServerFinishedLoading;
}
