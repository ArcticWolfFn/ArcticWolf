#include "pch.h"
#include "GameModeBase.h"

AGameModeBase::AGameModeBase(UObject* InternalGameModeBaseObject) : InternalObject(InternalGameModeBaseObject)
{
}

void AGameModeBase::Setup()
{
	__super::Setup();
}
