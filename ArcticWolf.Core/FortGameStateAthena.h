#pragma once

#include "FortGameStateZone.h"
#include "PlaylistPropertyArray.h"

enum class EAthenaGamePhase : uint8_t {
	None,
	Setup,
	Warmup,
	Aircraft,
	SafeZones,
	EndGame,
	Count,
	EAthenaGamePhase_MAX,
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

	FPlaylistPropertyArray* CurrentPlaylistInfo = nullptr;
	EAthenaGamePhase* GamePhase = nullptr;

private:
	static int32_t Offset_CurrentPlaylistInfo;
	static int32_t Offset_GamePhase;

	UFunction* Fn_OnRep_CurrentPlaylistInfo;
	UFunction* Fn_OnRep_GamePhase;

	bool CanExec_OnRep_CurrentPlaylistInfo = false;
	bool CanExec_OnRep_GamePhase = false;
};

