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
	if (Util::IsBadReadPtr(Playlist)) {
		PLOGE << "Playlist is nullptr";
		return;
	}
	if (Util::IsBadReadPtr(InternalObject)) {
		PLOGE << "InternalObject is nullptr";
		return;
	}
	InternalObject->BasePlaylist = Playlist;
}

void FPlaylistPropertyArray::SetOverridePlaylist(InternalUObject* Playlist)
{
	InternalObject->OverridePlaylist = Playlist;
}
