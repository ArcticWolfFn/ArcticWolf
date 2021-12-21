#pragma once
#include "finder.h"
#include "framework.h"
#include "ue4.h"

static class UFunctions
{
public:
	static auto SetTimeOfDay(float Time);

	static void TeleportToSpawn();

	static void TeleportToMain();

	static void TeleportToCoords(float X, float Y, float Z);

	static void DestroyAllHLODs();

	static void Travel(const wchar_t* url);

	static void StartMatch();

	//Simulates the server telling the game that it's ready to start match
	static void ServerReadyToStartMatch();

	static void SetPlaylist();

	static void SetGamePhase();

	//static void LoadAndStreamInLevel(const wchar_t* EventSequenceMap);

	//static void Play(const wchar_t* AnimationPlayerFullName);

	static void ConsoleLog(std::wstring message);

	static void DestoryActor(UObject* actor);

	//static void PlayCustomPlayPhaseAlert();

	static auto StaticLoadObjectEasy(UClass* inClass, const wchar_t* inName, UObject* inOuter = nullptr);
};
