#include "pch.h"
#include "FortGameStateBase.h"

AFortGameStateBase::AFortGameStateBase(InternalUObject*& InternalObject) : APlayspaceGameState(InternalObject)
{
}

void AFortGameStateBase::Setup()
{
	__super::Setup();
}
