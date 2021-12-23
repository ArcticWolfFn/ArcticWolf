#pragma once

#include "FortGameStateZone.h"
#include "PlaylistPropertyArray.h"

enum class EAthenaGamePhase : uint8_t {
	None = 0,
	Setup = 1,
	Warmup = 2,
	Aircraft = 3,
	SafeZones = 4,
	EndGame = 5,
	Count = 6,
	EAthenaGamePhase_MAX = 7
};

class AFortGameStateAthena : public AFortGameStateZone
{
public:
	AFortGameStateAthena();
	AFortGameStateAthena(AGameStateBase GameStateBase);

	void Setup() override;

	// (Final|Native|Protected)
	void OnRep_CurrentPlaylistInfo();

	// (Final|Native|Protected)
	void OnRep_GamePhase(EAthenaGamePhase OldGamePhase);

	FPlaylistPropertyArray CurrentPlaylistInfo = NULL;
	EAthenaGamePhase* GamePhase = nullptr;

private:
	static int32_t Offset_CurrentPlaylistInfo;
	static int32_t Offset_GamePhase;

	UFunction* Fn_OnRep_CurrentPlaylistInfo = nullptr;
	UFunction* Fn_OnRep_GamePhase = nullptr;

	bool CanExec_OnRep_CurrentPlaylistInfo = false;
	bool CanExec_OnRep_GamePhase = false;
};

