#include "pch.h"
#include "Player.h"

UPlayer::UPlayer(ObjectFinder* PlayerFinder) : PlayerFinder(PlayerFinder)
{

}

void UPlayer::Setup()
{
	auto pControllerFinder = PlayerFinder->Find(XOR(L"PlayerController"));
	PlayerController = new APlayerController(&pControllerFinder);
	PlayerController->Setup();
}

APlayerController* UPlayer::GetPlayerController()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	auto playerFinder = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

	auto pControllerFinder = playerFinder.Find(XOR(L"PlayerController"));

	PlayerController = new APlayerController(&pControllerFinder);
	PlayerController->Setup();

	return PlayerController;
}
