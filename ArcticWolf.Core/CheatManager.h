#pragma once

#include "Actor.h"

class UCheatManager : GIObject
{
public:
	UCheatManager(InternalUObject*& InternalCheatManager);

	void Setup() override;

	// (Exec|Native|Public)
	void BugItGo(float X, float Y, float Z, float Pitch, float Yaw, float Roll);

	// (Exec|Native|Public)
	void DestroyAll(AActor* aClass);

	void Summon(FString ClassName);

private:
	static UFunction* Fn_BugItGo;
	static UFunction* Fn_DestroyAll;
	static UFunction* Fn_Summon;

	InternalUObject*& InternalCheatManager;

	static bool CanExec_BugItGo;
	static bool CanExec_DestroyAll;
	static bool CanExec_Summon;
};

