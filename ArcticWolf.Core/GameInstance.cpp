#include "pch.h"
#include "GameInstance.h"

#include "Finder.h"

void UGameInstance::Setup()
{
	__super::Setup();

	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameInstanceFinder = EngineFinder.Find(XOR(L"GameInstance"));

	// Players is plural, but it works for now
	auto localPlayer = ULocalPlayer(GameInstanceFinder.Find(XOR(L"LocalPlayers")));
	localPlayer.Setup();
	// ToDo: doesn't seem to actually work. Maybe use objects instead of pointers
	LocalPlayers.push_back(&localPlayer);
}
