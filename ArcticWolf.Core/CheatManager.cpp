#include "pch.h"
#include "CheatManager.h"

UFunction* UCheatManager::Fn_BugItGo = nullptr;
UFunction* UCheatManager::Fn_DestroyAll = nullptr;

bool UCheatManager::CanExec_BugItGo = false;
bool UCheatManager::CanExec_DestroyAll = false;

UCheatManager::UCheatManager(InternalUObject*& InternalCheatManager) : InternalCheatManager(InternalCheatManager)
{
}

void UCheatManager::Setup()
{
	SetPointer(XOR(L"Function /Script/Engine.CheatManager:BugItGo"), &Fn_BugItGo, &CanExec_BugItGo);
	SetPointer(XOR(L"Function /Script/Engine.CheatManager:DestroyAll"), &Fn_DestroyAll, &CanExec_DestroyAll);
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

void UCheatManager::DestroyAll(AActor* aClass)
{
	if (!CanExec_DestroyAll) return;

	struct Params
	{
		AActor* aClass;
	};

	auto params = new Params();
	params->aClass = aClass;

	if (Util::IsBadReadPtr(aClass)) {
		PLOGE << "aClass is nullptr";
	}

	if (Util::IsBadReadPtr(InternalCheatManager)) {
		PLOGE << "InternalCheatManager is nullptr";
	}

	if (Util::IsBadReadPtr(Fn_DestroyAll)) {
		PLOGE << "Fn_DestroyAll is nullptr";
	}

	ProcessEvent(InternalCheatManager, Fn_DestroyAll, &params);
}
