#include "pch.h"
#include "CheatManager.h"

UCheatManager::UCheatManager(UObject* InternalCheatManager) : InternalCheatManager(InternalCheatManager)
{
}

void UCheatManager::Setup()
{
	GIObject::SetPointer(XOR(L"Function /Script/Engine.CheatManager:BugItGo"), &Fn_BugItGo, &CanExec_BugItGo);
}

void UCheatManager::BugItGo(float X, float Y, float Z, float Pitch, float Yaw, float Roll)
{
	if (!CanExec_BugItGo) return;

	struct Params
	{
		float X;
		float Y;
		float Z;
		float Pitch;
		float Yaw;
		float Roll;
	};

	auto params = Params();
	params.X = X;
	params.Y = Y;
	params.Z = Z;
	params.Pitch = Pitch;
	params.Yaw = Yaw;
	params.Roll = Roll;

	ProcessEvent(InternalCheatManager, Fn_BugItGo, &params);
}
