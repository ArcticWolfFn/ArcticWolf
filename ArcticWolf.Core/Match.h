#pragma once

#include "mods.h"
#include "sdk.h"
#include "mods.h"
#include "FortniteGame.h"

inline std::vector<std::wstring> gWeapons;
inline std::vector<std::wstring> gBlueprints;
inline std::vector<std::wstring> gMeshes;
inline UObject* gPlaylist;

static class Match
{
	static bool bIsInit;
	static bool bIsStarted;
	static bool bIsPlayerInit;

	static bool bHasJumped;
	static bool bHasShowedPickaxe;

	static bool bWantsToJump;
	static bool bWantsToSkydive;
	static bool bWantsToOpenGlider;
	static bool bWantsToShowPickaxe;

	static Player NeoPlayer;

	static void Start(const wchar_t* MapToPlayOn);

	static void Stop();

	static void LoadMoreClasses();

	static void InitCombos();

	static void Thread();

	static void Init();
};
