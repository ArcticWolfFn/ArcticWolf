#include "pch.h"
#include "GameMode.h"

AGameMode::AGameMode(AGameModeBase GameModeBase) : AGameModeBase(GameModeBase.InternalObject)
{
	this->Setup();
}

AGameMode::AGameMode(InternalUObject* InternalObject) : AGameModeBase(InternalObject)
{
}

void AGameMode::Setup()
{
	__super::Setup();

	SetPointer(XOR(L"Function /Script/Engine.GameMode:StartMatch"), &Fn_StartMatch, &CanExec_StartMatch);
	SetPointer(XOR(L"Function /Script/Engine.GameMode:Say"), &Fn_StartMatch, &CanExec_Say);
}

void AGameMode::StartMatch()
{
	if (!CanExec_StartMatch) return;

	if (Util::IsBadReadPtr(InternalObject))
	{
		PLOGE << "InternalObject is nullptr";
	}

	if (Util::IsBadReadPtr(Fn_StartMatch))
	{
		PLOGE << "Fn_StartMatch is nullptr";
	}

	ProcessNoParamsEvent(InternalObject, Fn_StartMatch);
}

void AGameMode::Say(FString Msg)
{
	if (!CanExec_Say) return;

	struct Params {
		FString Msg;
	};

	auto params = Params();
	params.Msg = Msg;

	ProcessEvent(InternalObject, Fn_Say, &params);
}
