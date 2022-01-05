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

private:
	static UFunction* Fn_BugItGo;
	static UFunction* Fn_DestroyAll;

	InternalUObject*& InternalCheatManager;

	static bool CanExec_BugItGo;
	static bool CanExec_DestroyAll;
};

