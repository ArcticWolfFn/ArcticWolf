#include "pch.h"
#include "Controller.h"

#include "Pawn.h"

UFunction* AController::Fn_Possess = nullptr;

bool AController::CanExec_Possess = false;

AController::AController() : InternalObject(toPointerReference(nullptr))
{
}

AController::AController(ObjectFinder* ControllerFinder) : InternalObject(ControllerFinder->GetObj())
{
}

AController::AController(InternalUObject*& InternalObject) : InternalObject(InternalObject)
{
}

void AController::Setup()
{
	__super::Setup();

	SetPointer(XOR(L"Function /Script/Engine.Controller:Possess"), &Fn_Possess, &CanExec_Possess);
}

void AController::Possess(APawn* InPawn)
{
	struct Params {
		InternalUObject* InPawn;
	};

	auto params = Params();
	params.InPawn = InPawn->InternalObject;

	ProcessEvent(InternalObject, Fn_Possess, &params);
}
