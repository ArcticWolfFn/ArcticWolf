#pragma once

#include "sdk.h"
#include "NeoPlayer.h"
#include "FortPlaylistAthena.h"

inline std::vector<std::wstring> gWeapons;
inline std::vector<std::wstring> gBlueprints;
inline std::vector<std::wstring> gMeshes;
inline UFortPlaylistAthena* gPlaylist;

static class Match
{
public:
	static bool bIsInit;
	static bool bIsStarted;
	static bool bIsPlayerInit;

	static bool bHasJumped;
	static bool bHasShowedPickaxe;

	static bool bWantsToJump;
	static bool bWantsToSkydive;
	static bool bWantsToOpenGlider;
	static bool bWantsToShowPickaxe;

	static NeoPlayerClass NeoPlayer;

	static void Start(const wchar_t* MapToPlayOn);

	static void Stop();

	static void LoadMoreClasses();

	static void InitCombos();

	static void Thread();

	static void Init();
};
