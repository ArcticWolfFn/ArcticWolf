#include "pch.h"
#include "FortKismetLibrary.h"

UFunction* FortniteGame::UFortKismetLibrary::Fn_SetTimeOfDay = nullptr;

void FortniteGame::UFortKismetLibrary::SetTimeOfDay(UObject* WorldContextObject, float TimeOfDay)
{
	struct Params
	{
		UObject* WorldContextObject;
		float TimeOfDay;
	};

	auto params = Params();
	params.WorldContextObject = WorldContextObject;
	params.TimeOfDay = TimeOfDay;

	ProcessEvent(InternalFortKismetLibrary, Fn_SetTimeOfDay, &params);
}

void FortniteGame::UFortKismetLibrary::Setup()
{
	__super::Setup();

	bool foundKismetLib = false;
	SetPointer(XOR(L"FortKismetLibrary /Script/FortniteGame.Default__FortKismetLibrary"), &InternalFortKismetLibrary, &foundKismetLib);

	if (!foundKismetLib) 
	{
		return;
	}

	SetPointer(XOR(L"Function /Script/FortniteGame.FortKismetLibrary:SetTimeOfDay"), &Fn_SetTimeOfDay);
}
