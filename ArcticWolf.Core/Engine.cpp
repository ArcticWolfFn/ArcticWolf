#include "pch.h"
#include "Engine.h"
#include "Finder.h"

UEngine::UEngine() : GameViewport(UGameViewportClient(ObjectFinder::EntryPoint(uintptr_t(GEngine)).Find(XOR(L"GameViewport"))))
{
}

void UEngine::Setup()
{
	__super::Setup();

	GameViewport.Setup();

	GameInstance.Setup();
}
