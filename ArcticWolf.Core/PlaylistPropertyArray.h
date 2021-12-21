#pragma once

#include "FortPlaylistAthena.h"

// NOT INTENDED FOR PUBLIC USE
struct INTERNALFPlaylistPropertyArray
{
	char padding[0x0120];
	UObject* BasePlaylist;
	UObject* OverridePlaylist;
};

class FPlaylistPropertyArray : GIObject
{
public:
	FPlaylistPropertyArray(INTERNALFPlaylistPropertyArray* InternalObject);

	void Setup() override;

	void SetBasePlaylist(UFortPlaylistAthena* Playlist);
	void SetOverridePlaylist(UFortPlaylistAthena* Playlist);

private:
	INTERNALFPlaylistPropertyArray* InternalObject;
};

