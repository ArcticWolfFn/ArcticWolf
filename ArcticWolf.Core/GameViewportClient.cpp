#include "pch.h"
#include "GameViewportClient.h"

UGameViewportClient::UGameViewportClient(ObjectFinder GameViewportClientFinder) : InternalFinder(GameViewportClientFinder),
World(UWorld(InternalFinder.Find(L"World")))
{
}

void UGameViewportClient::Setup()
{
	World.Setup();
}
