#include "pch.h"
#include "Actor.h"



AActor::AActor()
{
}

// Casting
AActor::AActor(UObject object)
{
	InternalObject = &object;
	Setup();
}

void AActor::Setup()
{
	SetPointer(XOR(L"Function /Script/Engine.Actor:K2_DestroyActor"), &Fn_K2_DestroyActor, &CanExec_K2_DestroyActor);
}

void AActor::K2_DestroyActor()
{
	if (!CanExec_K2_DestroyActor) return;

	ProcessNoParamsEvent(InternalObject, Fn_K2_DestroyActor);
}
