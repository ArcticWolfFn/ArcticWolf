#include "pch.h"
#include "PlayerController.h"
#include "Finder.h"



APlayerController::APlayerController(ObjectFinder* PlayerControllerFinder) : PlayerControllerFinder(PlayerControllerFinder)
{
}

void APlayerController::Setup()
{
	__super::Setup();

	InternalObject = PlayerControllerFinder->GetObj();

	if (Util::IsBadReadPtr(InternalObject)) return;

	GIObject::SetPointer(XOR(L"Function /Script/Engine.PlayerController:SwitchLevel"), &Fn_SwitchLevel, &CanExec_SwitchLevel);

	CheatManager = UCheatManager(PlayerControllerFinder->Find(XOR(L"CheatManager")).GetObj());
	CheatManager.Setup();
}

void APlayerController::SwitchLevel(FString URL)
{
	if (!CanExec_SwitchLevel) return;

	struct Params
	{
		FString URL;
	};

	auto params = Params();
	params.URL = URL;

	ProcessEvent(InternalObject, Fn_SwitchLevel, &params);
}
