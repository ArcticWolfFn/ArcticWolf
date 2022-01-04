#include "pch.h"
#include "PlayerController.h"
#include "Finder.h"

APlayerController::APlayerController() : InternalObject(toPointerReference(nullptr))
{
}

APlayerController::APlayerController(ObjectFinder* PlayerControllerFinder) : PlayerControllerFinder(PlayerControllerFinder), InternalObject(toPointerReference(nullptr))
{
	InternalObject = PlayerControllerFinder->GetObj();
}

APlayerController::APlayerController(APlayerController* PlayerController) : PlayerControllerFinder(PlayerController->PlayerControllerFinder), 
InternalObject(PlayerController->PlayerControllerFinder->GetObj())
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
