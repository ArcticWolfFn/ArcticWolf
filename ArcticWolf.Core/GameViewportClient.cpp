#include "pch.h"
#include "GameViewportClient.h"

UGameViewportClient::UGameViewportClient(ObjectFinder* GameViewportClientFinder) : InternalFinder(GameViewportClientFinder)
{
}

void UGameViewportClient::Setup()
{
	auto worldFinder = InternalFinder->Find(L"World");
	auto world = UWorld(&worldFinder);
	World = &world;
}
