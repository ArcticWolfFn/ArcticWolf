#include "pch.h"
#include "World.h"
#include "GameMode.h"

FActorSpawnParameters::FActorSpawnParameters() : Name(), Template(nullptr), Owner(nullptr), 
Instigator(nullptr), OverrideLevel(nullptr), SpawnCollisionHandlingOverride(), bRemoteOwned(0), 
bNoFail(0), bDeferConstruction(0), bAllowDuringConstructionScript(0), NameMode(), ObjectFlags()
{
}

bool FActorSpawnParameters::IsRemoteOwned() const
{
    return bRemoteOwned;
}

// ToDo: World is dynamic!
UWorld::UWorld(ObjectFinder GameViewportFinder) : GameViewportFinder(GameViewportFinder), InternalFinder(GameViewportFinder.Find(L"World"))
{
}

void UWorld::Setup()
{
    AuthorityGameMode = new AGameMode(InternalFinder.Find(L"AuthorityGameMode").GetObj());
    AuthorityGameMode->Setup();

    GameState = new AGameStateBase(InternalFinder.Find(XOR(L"GameState")).GetObj());
    GameState->Setup();
}

AGameStateBase* UWorld::GetGameState()
{
    UpdateProps();
    return GameState;
}

// ToDo: replace this with a listener for the UGameInstance::OnWorldChanged event
void UWorld::UpdateProps()
{
    ObjectFinder WorldFinder = GameViewportFinder.Find(XOR(L"World"));

    GameState = new AGameStateBase(WorldFinder.Find(XOR(L"GameState")).GetObj());
    GameState->Setup();

    AuthorityGameMode = new AGameMode(WorldFinder.Find(L"AuthorityGameMode").GetObj());
    AuthorityGameMode->Setup();

    PLOGV.printf("World is %ws", UE4::GetObjectName(WorldFinder.GetObj()));
}
