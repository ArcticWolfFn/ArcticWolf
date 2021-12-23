#include "pch.h"
#include "PlayerController.h"
#include "Finder.h"


APlayerController::APlayerController()
{
}

APlayerController::APlayerController(ObjectFinder* PlayerControllerFinder) : PlayerControllerFinder(PlayerControllerFinder)
{
}

void APlayerController::Setup()
{
	__super::Setup();

	InternalObject = PlayerControllerFinder->GetObj();

	if (Util::IsBadReadPtr(InternalObject)) return;

	SetPointer(XOR(L"Function /Script/Engine.PlayerController:SwitchLevel"), &Fn_SwitchLevel, &CanExec_SwitchLevel);

	CheatManager = UCheatManager(PlayerControllerFinder->Find(XOR(L"CheatManager")).GetObj());
	CheatManager.Setup();
}

void APlayerController::SwitchLevel(FString URL)
{
	if (!this->CanExec_SwitchLevel) return;

	struct Params
	{
		FString URL;
	};

	auto params = Params();
	params.URL = URL;

	if (Util::IsBadReadPtr(InternalObject))
	{
		PLOGE << "InternalObject is nullptr";
	}

	if (Util::IsBadReadPtr(Fn_SwitchLevel))
	{
		PLOGE << "Fn_SwitchLevel is nullptr";
	}

	ProcessEvent(InternalObject, Fn_SwitchLevel, &params);
}
