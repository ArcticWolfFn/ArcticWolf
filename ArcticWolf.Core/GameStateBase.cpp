#include "pch.h"
#include "GameStateBase.h"

AGameStateBase::AGameStateBase()
{
}

AGameStateBase::AGameStateBase(InternalUObject* InternalObject) : InternalObject(InternalObject)
{
}

void AGameStateBase::Setup()
{
	__super::Setup();
}

InternalUObject* AGameStateBase::GetInternalObject()
{
	return this->InternalObject;
}
