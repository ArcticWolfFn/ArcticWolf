#include "pch.h"
#include "Player.h"

UPlayer::UPlayer(ObjectFinder PlayerFinder) : PlayerFinder(PlayerFinder)
{

}

void UPlayer::Setup()
{
	auto pControllerFinder = PlayerFinder.Find(XOR(L"PlayerController"));
	auto pController = APlayerController(&pControllerFinder);
	PlayerController = &pController;
}