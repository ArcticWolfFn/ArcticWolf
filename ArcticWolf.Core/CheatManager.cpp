#include "pch.h"
#include "CheatManager.h"

UFunction* UCheatManager::Fn_BugItGo = nullptr;
UFunction* UCheatManager::Fn_DestroyAll = nullptr;
UFunction* UCheatManager::Fn_Summon = nullptr;

bool UCheatManager::CanExec_BugItGo = false;
bool UCheatManager::CanExec_DestroyAll = false;
bool UCheatManager::CanExec_Summon = false;

UCheatManager::UCheatManager(InternalUObject*& InternalCheatManager) : InternalCheatManager(InternalCheatManager)
{
}

void UCheatManager::Setup()
{
	SetPointer(XOR(L"Function /Script/Engine.CheatManager:BugItGo"), &Fn_BugItGo, &CanExec_BugItGo);
	SetPointer(XOR(L"Function /Script/Engine.CheatManager:DestroyAll"), &Fn_DestroyAll, &CanExec_DestroyAll);
	SetPointer(XOR(L"Function /Script/Engine.CheatManager:Summon"), &Fn_Summon, &CanExec_Summon);
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
		InternalUObject* aClass;
	};

	auto params = new Params();
	params->aClass = aClass->InternalObject;

	ProcessEvent(InternalCheatManager, Fn_DestroyAll, &params);
}

void UCheatManager::Summon(FString ClassName)
{
	if (!CanExec_Summon) return;

	struct Params
	{
		FString ClassName;
	};

	auto params = new Params();
	params->ClassName = ClassName;

	ProcessEvent(InternalCheatManager, Fn_Summon, &params);
}
