#include "pch.h"
#include "GameInstance.h"

#include "Finder.h"

void UGameInstance::Setup()
{
	__super::Setup();

	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameInstanceFinder = EngineFinder.Find(XOR(L"GameInstance"));

	// Players is plural, but it works for now
	ObjectFinder LocalPlayerFinder = GameInstanceFinder.Find(XOR(L"LocalPlayers"));
	auto localPlayer = ULocalPlayer(&LocalPlayerFinder);
	localPlayer.Setup();

	LocalPlayers.push_back(localPlayer);
}
