#pragma once
class UCheatManager : GIObject
{
public:
	UCheatManager(UObject* InternalCheatManager);

	void Setup() override;

	// (Exec|Native|Public)
	void BugItGo(float X, float Y, float Z, float Pitch, float Yaw, float Roll);

private:
	UFunction* Fn_BugItGo = nullptr;

	UObject* InternalCheatManager = nullptr;

	bool CanExec_BugItGo = false;
};

