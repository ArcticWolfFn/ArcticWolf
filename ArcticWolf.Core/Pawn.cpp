#include "pch.h"
#include "Pawn.h"

APawn::APawn() : AActor()
{
}

APawn::APawn(InternalUObject*& object) : AActor(object)
{
}

void APawn::Setup()
{
	__super::Setup();
}
