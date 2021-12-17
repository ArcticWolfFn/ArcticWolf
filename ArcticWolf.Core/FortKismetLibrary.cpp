#include "pch.h"
#include "FortKismetLibrary.h"
#include "Object.cpp"

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
	bool foundKismetLib = false;
	GIObject::SetPointer(XOR(L"FortKismetLibrary /Script/FortniteGame.Default__FortKismetLibrary"), &InternalFortKismetLibrary, &foundKismetLib);

	if (!foundKismetLib) 
	{
		return;
	}

	GIObject::SetPointer(XOR(L"FortKismetLibrary /Script/FortniteGame.Default__FortKismetLibrary"), &Fn_SetTimeOfDay);
}
