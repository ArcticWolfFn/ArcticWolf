#include "pch.h"
#include "GameViewportClient.h"

UGameViewportClient::UGameViewportClient(ObjectFinder GameViewportClientFinder) : InternalFinder(GameViewportClientFinder)
{
}

void UGameViewportClient::Setup()
{
	World = new UWorld(InternalFinder);
	World->Setup();
}
