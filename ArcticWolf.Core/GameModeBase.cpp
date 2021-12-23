#include "pch.h"
#include "GameModeBase.h"

AGameModeBase::AGameModeBase(InternalUObject* InternalGameModeBaseObject) : InternalObject(InternalGameModeBaseObject)
{
}

void AGameModeBase::Setup()
{
	__super::Setup();
}
