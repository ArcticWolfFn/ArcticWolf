#include "pch.h"
#include "FortGameStateAthena.h"

void AFortGameStateAthena::Setup()
{
	if (Offset_CurrentPlaylistInfo == 0)
	{
		Offset_CurrentPlaylistInfo = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortGameStateAthena"), XOR(L"CurrentPlaylistInfo"));
	}

	CurrentPlaylistInfo = reinterpret_cast<INTERNALFPlaylistPropertyArray*>(reinterpret_cast<uintptr_t>(InternalObject) + Offset_CurrentPlaylistInfo);
}

void AFortGameStateAthena::OnRep_CurrentPlaylistInfo()
{
}
