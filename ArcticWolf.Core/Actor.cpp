#include "pch.h"
#include "Actor.h"

UFunction* AActor::Fn_K2_DestroyActor = nullptr;

bool AActor::CanExec_K2_DestroyActor = false;

AActor::AActor() : InternalObject(toPointerReference(nullptr))
{
}

// Casting
AActor::AActor(InternalUObject* object) : InternalObject(object)
{
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
