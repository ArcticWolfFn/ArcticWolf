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

UWorld::UWorld(ObjectFinder* WorldFinder) : InternalFinder(WorldFinder)
{
}

void UWorld::Setup()
{
    auto authorityGameMode = AGameMode(InternalFinder->Find(L"AuthorityGameMode").GetObj());
    authorityGameMode.Setup();
    AuthorityGameMode = &authorityGameMode;

    this->GameState = AGameStateBase(InternalFinder->Find(XOR(L"GameState")).GetObj());
    this->GameState.Setup();
}

AGameStateBase* UWorld::GetGameState()
{
    return &GameState;
}
