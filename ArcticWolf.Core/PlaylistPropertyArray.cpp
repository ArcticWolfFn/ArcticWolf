#include "pch.h"
#include "PlaylistPropertyArray.h"

FPlaylistPropertyArray::FPlaylistPropertyArray(INTERNALFPlaylistPropertyArray* InternalObject) : InternalObject(InternalObject)
{
}

void FPlaylistPropertyArray::Setup()
{
	__super::Setup();
}

void FPlaylistPropertyArray::SetBasePlaylist(UFortPlaylistAthena* Playlist)
{
	InternalObject->BasePlaylist = Playlist;
}

void FPlaylistPropertyArray::SetOverridePlaylist(UFortPlaylistAthena* Playlist)
{
	InternalObject->OverridePlaylist = Playlist;
}
