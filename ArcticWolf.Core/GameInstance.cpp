#include "pch.h"
#include "GameInstance.h"

#include "Finder.h"

void UGameInstance::Setup()
{
	__super::Setup();

	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameInstanceFinder = EngineFinder.Find(XOR(L"GameInstance"));
}
