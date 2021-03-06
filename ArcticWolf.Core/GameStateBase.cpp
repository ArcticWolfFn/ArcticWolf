#include "pch.h"
#include "GameStateBase.h"

AGameStateBase::AGameStateBase() : InternalObject(toPointerReference(nullptr))
{
}

AGameStateBase::AGameStateBase(InternalUObject*& InternalObject) : InternalObject(InternalObject)
{
}

void AGameStateBase::Setup()
{
	__super::Setup();
}

InternalUObject*& AGameStateBase::GetInternalObjectRef()
{
	return this->InternalObject;
}

InternalUObject* AGameStateBase::GetInternalObject()
{
	return this->InternalObject;
}
