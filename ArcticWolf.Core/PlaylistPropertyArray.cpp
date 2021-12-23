#include "pch.h"
#include "PlaylistPropertyArray.h"

FPlaylistPropertyArray::FPlaylistPropertyArray(INTERNALFPlaylistPropertyArray* InternalObject) : InternalObject(InternalObject)
{
}

void FPlaylistPropertyArray::Setup()
{
	__super::Setup();
}

void FPlaylistPropertyArray::SetBasePlaylist(InternalUObject* Playlist)
{
	InternalObject->BasePlaylist = Playlist;
}

void FPlaylistPropertyArray::SetOverridePlaylist(InternalUObject* Playlist)
{
	InternalObject->OverridePlaylist = Playlist;
}
