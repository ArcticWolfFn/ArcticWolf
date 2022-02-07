#include "pch.h"
#include "World.h"
#include "GameMode.h"

UFunction* UWorld::Fn_SpawnActor = nullptr;
bool UWorld::CanExec_SpawnActor = false;

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

    SetPointer(XOR(L"Function /Script/Engine.World:SpawnActor"), &Fn_SpawnActor, &CanExec_SpawnActor);
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
}

void UWorld::SpawnActorEasy(InternalUClass* Class, FVector Location, FQuat Rotation)
{
    FTransform Transform;
    Transform.Scale3D = FVector(1, 1, 1);
    Transform.Translation = Location;
    Transform.Rotation = Rotation;

    ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
    ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
    ObjectFinder WorldFinder = GameViewPortClientFinder.Find(XOR(L"World"));

    struct Params {
        InternalUClass* Class = nullptr;
        const FTransform* UserTransformPtr = nullptr;
        const FActorSpawnParameters& SpawnParameters = FActorSpawnParameters();
    };

    auto params = new Params();
    params->Class = Class;
    params->UserTransformPtr = &Transform;

    ProcessEvent(WorldFinder.GetObj(), &Fn_SpawnActor, &params);
}
