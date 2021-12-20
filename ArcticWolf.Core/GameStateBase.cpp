#include "pch.h"
#include "GameStateBase.h"

AGameStateBase::AGameStateBase(UObject* InternalObject) : InternalObject(InternalObject)
{
}

void AGameStateBase::Setup()
{
	__super::Setup();
}
