#include "pch.h"
#include "PlayerController.h"
#include "Finder.h"

UFunction* APlayerController::Fn_SwitchLevel = nullptr;
UFunction* APlayerController::Fn_LocalTravel = nullptr;

bool APlayerController::CanExec_SwitchLevel = false;
bool APlayerController::CanExec_LocalTravel = false;

APlayerController::APlayerController() : AController()
{
}

APlayerController::APlayerController(ObjectFinder* PlayerControllerFinder) : AController(PlayerControllerFinder), PlayerControllerFinder(PlayerControllerFinder)
{
}

APlayerController::APlayerController(APlayerController* PlayerController) : AController(PlayerController->PlayerControllerFinder), 
PlayerControllerFinder(PlayerController->PlayerControllerFinder)
{
	CheatManager = PlayerController->CheatManager;
	Setup();
}

void APlayerController::Setup()
{
	__super::Setup();

	if (Util::IsBadReadPtr(InternalObject)) return;

	SetPointer(XOR(L"Function /Script/Engine.PlayerController:SwitchLevel"), &Fn_SwitchLevel, &CanExec_SwitchLevel);
	SetPointer(XOR(L"Function /Script/Engine.PlayerController:LocalTravel"), &Fn_LocalTravel, &CanExec_LocalTravel);
	SetPointer(XOR(L"Function /Script/Engine.PlayerController:LocalTravel"), &Fn_LocalTravel, &CanExec_LocalTravel);

	CheatManager = new UCheatManager(PlayerControllerFinder->Find(XOR(L"CheatManager")).GetObj());
	CheatManager->Setup();
}

void APlayerController::SwitchLevel(FString URL)
{
	if (!this->CanExec_SwitchLevel)
	{
		PLOGE << "CanExec_SwitchLevel is false";
		return;
	}

	struct Params
	{
		FString URL;
	};

	auto params = Params();
	params.URL = URL;

	ProcessEvent(InternalObject, Fn_SwitchLevel, &params);
}

void APlayerController::LocalTravel(FString URL)
{
	if (!this->CanExec_LocalTravel) return;

	struct Params
	{
		FString URL;
	};

	auto params = Params();
	params.URL = URL;

	ProcessEvent(InternalObject, Fn_LocalTravel, &params);
}