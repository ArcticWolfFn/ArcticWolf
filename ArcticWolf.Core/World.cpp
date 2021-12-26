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

UWorld::UWorld(ObjectFinder WorldFinder) : InternalFinder(WorldFinder), GameState(AGameStateBase(InternalFinder.Find(XOR(L"GameState")).GetObj()))
{
}

void UWorld::Setup()
{
    AuthorityGameMode = AGameMode(InternalFinder.Find(L"AuthorityGameMode").GetObj());
    AuthorityGameMode.Setup();

    this->GameState.Setup();
}

AGameStateBase* UWorld::GetGameState()
{
    return &GameState;
}
