#pragma once

#include "FortGameStateZone.h"
#include "PlaylistPropertyArray.h"

class AFortGameStateAthena : public AFortGameStateZone
{
public:
	void Setup() override;

	FPlaylistPropertyArray CurrentPlaylistInfo;

	// (Final|Native|Protected)
	void OnRep_CurrentPlaylistInfo();

private:
	static int32_t Offset_CurrentPlaylistInfo;
};

