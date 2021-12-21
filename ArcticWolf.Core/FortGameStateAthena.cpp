#include "pch.h"
#include "FortGameStateAthena.h"

int32_t AFortGameStateAthena::Offset_CurrentPlaylistInfo = 0;
int32_t AFortGameStateAthena::Offset_GamePhase = 0;

AFortGameStateAthena::AFortGameStateAthena()
{
}

// Only used for manual casting
AFortGameStateAthena::AFortGameStateAthena(AGameStateBase GameStateBase)
{
	this->InternalObject = GameStateBase.GetInternalObject();
	this->Setup();
}

void AFortGameStateAthena::Setup()
{
	__super::Setup();

	if (Offset_CurrentPlaylistInfo == 0)
	{
		Offset_CurrentPlaylistInfo = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortGameStateAthena"), XOR(L"CurrentPlaylistInfo"));
	}

	if (Offset_GamePhase == 0)
	{
		Offset_GamePhase = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortGameStateAthena"), XOR(L"GamePhase"));
	}

	auto InternalCurrentPlaylistInfo = reinterpret_cast<INTERNALFPlaylistPropertyArray*>(reinterpret_cast<uintptr_t>(InternalObject) + Offset_CurrentPlaylistInfo);
	auto currentPlaylistInfo = FPlaylistPropertyArray(InternalCurrentPlaylistInfo);
	currentPlaylistInfo.Setup();
	CurrentPlaylistInfo = &currentPlaylistInfo;

	GamePhase = reinterpret_cast<EAthenaGamePhase*>(reinterpret_cast<uintptr_t>(InternalObject) + Offset_GamePhase);

	SetPointer(XOR(L"Function /Script/FortniteGame.FortGameStateAthena:OnRep_CurrentPlaylistInfo"), &Fn_OnRep_CurrentPlaylistInfo, &CanExec_OnRep_CurrentPlaylistInfo);
	SetPointer(XOR(L"Function /Script/FortniteGame.FortGameStateAthena:OnRep_GamePhase"), &Fn_OnRep_GamePhase, &CanExec_OnRep_GamePhase);
}

void AFortGameStateAthena::OnRep_CurrentPlaylistInfo()
{
	if (!CanExec_OnRep_CurrentPlaylistInfo) return;
	ProcessNoParamsEvent(InternalObject, Fn_OnRep_CurrentPlaylistInfo);
}

void AFortGameStateAthena::OnRep_GamePhase(EAthenaGamePhase OldGamePhase)
{
	if (!CanExec_OnRep_GamePhase) return;

	struct Params {
		EAthenaGamePhase OldGamePhase;
	};

	auto params = Params();
	params.OldGamePhase = OldGamePhase;

	ProcessEvent(InternalObject, Fn_OnRep_GamePhase, &params);
}
