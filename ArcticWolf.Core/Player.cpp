#include "pch.h"
#include "Player.h"

UPlayer::UPlayer(ObjectFinder PlayerFinder) : PlayerFinder(PlayerFinder)
{

}

void UPlayer::Setup()
{
	PlayerController = &APlayerController(&PlayerFinder.Find(XOR(L"PlayerController")));
}
