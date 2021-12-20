#include "pch.h"
#include "FortniteGameInstance.h"

void FortniteGameInstance::Setup()
{
	__super::Setup();

	FortKismetLibrary = UFortKismetLibrary();
	FortKismetLibrary.Setup();
}
