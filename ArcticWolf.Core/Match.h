#pragma once

#include "sdk.h"
#include "NeoPlayer.h"
#include "FortPlaylistAthena.h"

inline std::vector<std::wstring> gWeapons;
inline std::vector<std::wstring> gBlueprints;
inline std::vector<std::wstring> gMeshes;
inline InternalUObject* gPlaylist;

enum class EMatchState : uint8_t {
	InLobby,
	Loading,
	InMatch
};

class Match
{
public:
	bool bIsInit;
	bool bIsStarted;
	bool bIsPlayerInit;

	bool bHasJumped;
	bool bHasShowedPickaxe;

	bool bWantsToJump;
	bool bWantsToSkydive;
	bool bWantsToOpenGlider;
	bool bWantsToShowPickaxe;

	EMatchState MatchState = EMatchState::InLobby;

	NeoPlayerClass NeoPlayer;

	void Start(const wchar_t* MapToPlayOn);

	void Stop();

	void LoadMoreClasses();

	void InitCombos();
	
	// because CreaThread needs a static entry point
	static DWORD ThreadEntry(LPVOID* param);

	void Thread();

	void Init();
};

inline Match GMatch = Match();

