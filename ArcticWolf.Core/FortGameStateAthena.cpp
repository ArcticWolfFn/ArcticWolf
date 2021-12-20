#include "pch.h"
#include "FortGameStateAthena.h"

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

	auto InternalCurrentPlaylistInfo = reinterpret_cast<INTERNALFPlaylistPropertyArray*>(reinterpret_cast<uintptr_t>(InternalObject) + Offset_CurrentPlaylistInfo);
	auto currentPlaylistInfo = FPlaylistPropertyArray(InternalCurrentPlaylistInfo);
	currentPlaylistInfo.Setup();
	CurrentPlaylistInfo = &currentPlaylistInfo;

	SetPointer(XOR(L"Function /Script/FortniteGame.FortGameStateAthena:OnRep_CurrentPlaylistInfo"), &Fn_OnRep_CurrentPlaylistInfo, &CanExec_OnRep_CurrentPlaylistInfo);
}

void AFortGameStateAthena::OnRep_CurrentPlaylistInfo()
{
	if (!CanExec_OnRep_CurrentPlaylistInfo) return;
	ProcessNoParamsEvent(InternalObject, Fn_OnRep_CurrentPlaylistInfo);
}
