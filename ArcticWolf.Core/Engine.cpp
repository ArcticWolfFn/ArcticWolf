#include "pch.h"
#include "Engine.h"
#include "Finder.h"

void UEngine::Setup()
{
	__super::Setup();

	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));

	auto GameViewPortFinder = EngineFinder.Find(XOR(L"GameViewport"));
	GameViewport = new UGameViewportClient(GameViewPortFinder);
	GameViewport->Setup();

	GameInstance.Setup();
}
