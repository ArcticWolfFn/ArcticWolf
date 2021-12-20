#include "pch.h"
#include "GameStateBase.h"

AGameStateBase::AGameStateBase()
{
}

AGameStateBase::AGameStateBase(UObject* InternalObject) : InternalObject(InternalObject)
{
}

void AGameStateBase::Setup()
{
	__super::Setup();
}

UObject* AGameStateBase::GetInternalObject()
{
	return this->InternalObject;
}
