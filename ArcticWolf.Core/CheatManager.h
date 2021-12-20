#pragma once

#include "Actor.h"

class UCheatManager : GIObject
{
public:
	UCheatManager(UObject* InternalCheatManager);

	void Setup() override;

	// (Exec|Native|Public)
	void BugItGo(float X, float Y, float Z, float Pitch, float Yaw, float Roll);

	// (Exec|Native|Public)
	void DestroyAll(AActor* aClass);

private:
	UFunction* Fn_BugItGo = nullptr;
	UFunction* Fn_DestroyAll = nullptr;

	UObject* InternalCheatManager = nullptr;

	bool CanExec_BugItGo = false;
	bool CanExec_DestroyAll = false;
};

