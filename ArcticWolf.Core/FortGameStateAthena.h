#pragma once

#include "FortGameStateZone.h"
#include "PlaylistPropertyArray.h"

class AFortGameStateAthena : public AFortGameStateZone
{
public:
	AFortGameStateAthena();
	AFortGameStateAthena(AGameStateBase GameStateBase);

	void Setup() override;

	FPlaylistPropertyArray* CurrentPlaylistInfo = nullptr;

	// (Final|Native|Protected)
	void OnRep_CurrentPlaylistInfo();

private:
	static int32_t Offset_CurrentPlaylistInfo;

	UFunction* Fn_OnRep_CurrentPlaylistInfo;

	bool CanExec_OnRep_CurrentPlaylistInfo = false;
};

