#include "pch.h"
#include "FortGameStateZone.h"

AFortGameStateZone::AFortGameStateZone(InternalUObject*& InternalObject) : AFortGameState_InGame(InternalObject)
{
}

void AFortGameStateZone::Setup()
{
	__super::Setup();
}
