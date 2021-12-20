#include "pch.h"
#include "PlayerController.h"
#include "Finder.h"



APlayerController::APlayerController(ObjectFinder* PlayerControllerFinder) : PlayerControllerFinder(PlayerControllerFinder)
{
}

void APlayerController::Setup()
{
	__super::Setup();

	CheatManager = UCheatManager(PlayerControllerFinder->Find(XOR(L"CheatManager")).GetObj());
	CheatManager.Setup();
}
