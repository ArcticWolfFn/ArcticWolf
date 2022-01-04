#include "pch.h"
#include "Character.h"

ACharacter::ACharacter()
{
}

ACharacter::ACharacter(InternalUObject* InternalObject) : InternalObject(InternalObject)
{
}

void ACharacter::Setup()
{
	__super::Setup();
}
