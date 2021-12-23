#pragma once

#include "FortPlaylistAthena.h"

// NOT INTENDED FOR PUBLIC USE
struct INTERNALFPlaylistPropertyArray
{
	char padding[0x0120];
	InternalUObject* BasePlaylist;
	InternalUObject* OverridePlaylist;
};

class FPlaylistPropertyArray : GIObject
{
public:
	FPlaylistPropertyArray(INTERNALFPlaylistPropertyArray* InternalObject);

	void Setup() override;

	void SetBasePlaylist(InternalUObject* Playlist);
	void SetOverridePlaylist(InternalUObject* Playlist);

private:
	INTERNALFPlaylistPropertyArray* InternalObject;
};

