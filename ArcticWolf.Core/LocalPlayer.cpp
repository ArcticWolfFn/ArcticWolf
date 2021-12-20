#include "pch.h"
#include "LocalPlayer.h"

ULocalPlayer::ULocalPlayer(ObjectFinder LocalPlayerFinder) : UPlayer::UPlayer(LocalPlayerFinder)
{
}

void ULocalPlayer::Setup()
{
	__super::Setup();
}
