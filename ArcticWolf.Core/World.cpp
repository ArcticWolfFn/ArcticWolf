#include "pch.h"
#include "World.h"

FActorSpawnParameters::FActorSpawnParameters() : Name(), Template(nullptr), Owner(nullptr), 
Instigator(nullptr), OverrideLevel(nullptr), SpawnCollisionHandlingOverride(), bRemoteOwned(0), 
bNoFail(0), bDeferConstruction(0), bAllowDuringConstructionScript(0), NameMode(), ObjectFlags()
{
}

bool FActorSpawnParameters::IsRemoteOwned() const
{
    return bRemoteOwned;
}
