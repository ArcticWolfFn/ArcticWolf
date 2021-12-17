#include "pch.h"
#include "Functions.h"
#include "Class.h"

auto UFunctions::SetTimeOfDay(float Time)
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
	ObjectFinder WorldFinder = GameViewPortClientFinder.Find(XOR(L"World"));

	FortniteGame::FortKismetLibrary.SetTimeOfDay(WorldFinder.GetObj(), Time);
}

void UFunctions::TeleportToSpawn()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

	ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

	ObjectFinder CheatManagerFinder = PlayerControllerFinder.Find(XOR(L"CheatManager"));

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.CheatManager:BugItGo"));

	UCheatManager_BugItGo_Params params;
	params.X = -156128.36;
	params.Y = -159492.78;
	params.Z = -2996.30;
	params.Pitch = 0;
	params.Yaw = 0;
	params.Roll = 0;

	ProcessEvent(CheatManagerFinder.GetObj(), fn, &params);

	PLOGI << "Teleported to spawn island";
}

void UFunctions::TeleportToMain()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

	ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

	ObjectFinder CheatManagerFinder = PlayerControllerFinder.Find(XOR(L"CheatManager"));

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.CheatManager:BugItGo"));

	UCheatManager_BugItGo_Params params;
	params.X = 0;
	params.Y = 0;
	params.Z = 0;
	params.Pitch = 0;
	params.Yaw = 0;
	params.Roll = 0;

	ProcessEvent(CheatManagerFinder.GetObj(), fn, &params);
}

void UFunctions::TeleportToCoords(float X, float Y, float Z)
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

	ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

	ObjectFinder CheatManagerFinder = PlayerControllerFinder.Find(XOR(L"CheatManager"));

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.CheatManager:BugItGo"));

	UCheatManager_BugItGo_Params params;
	params.X = X;
	params.Y = Y;
	params.Z = Z;
	params.Pitch = 0;
	params.Yaw = 0;
	params.Roll = 0;

	ProcessEvent(CheatManagerFinder.GetObj(), fn, &params);
}

void UFunctions::DestroyAllHLODs()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

	ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

	ObjectFinder CheatManagerFinder = PlayerControllerFinder.Find(XOR(L"CheatManager"));

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.CheatManager:DestroyAll"));

	auto HLODSMActor = UE4::FindObject<UClass*>(XOR(L"Class /Script/FortniteGame.FortHLODSMActor"));

	UCheatManager_DestroyAll_Params params;
	params.Class = HLODSMActor;

	ProcessEvent(CheatManagerFinder.GetObj(), fn, &params);
	PLOGD << "HLODSM Actor was destroyed.";
}

void UFunctions::Travel(const wchar_t* url)
{
	PLOGD << "Travel: Called";

	if (url == nullptr) {
		PLOGE << "Travel: url is null";
		return;
	}

	PLOGD.printf("Travel: To Url: %s", std::wstring(url).c_str());

	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));

	ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

	ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.PlayerController:SwitchLevel"));

	if (fn == nullptr) {
		PLOGE << "Travel: SwitchLevel function is null";
		return;
	}

	const FString URL = APOLLO_TERRAIN;
	PLOGD.printf("Travel: To fixed url: %s", URL.ToWString());

	APlayerController_SwitchLevel_Params params;
	params.URL = URL;

	UObject* playerControllerObj = PlayerControllerFinder.GetObj();

	if (playerControllerObj == nullptr) {
		PLOGE << "Travel: playerControllerObj is null";
		return;
	}

	PLOGD << "Travel: Process Event Called";

	ProcessEvent(playerControllerObj, fn, &params);

	PLOGD << "Travel: Finished";
}

void UFunctions::StartMatch()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
	ObjectFinder WorldFinder = GameViewPortClientFinder.Find(L"World");
	ObjectFinder GameModeFinder = WorldFinder.Find(L"AuthorityGameMode");

	const auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.GameMode:StartMatch"));
	Empty_Params params;

	ProcessEvent(GameModeFinder.GetObj(), fn, &params);

	PLOGI << "Match started!";
}

//Simulates the server telling the game that it's ready to start match
void UFunctions::ServerReadyToStartMatch()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

	ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerController:ServerReadyToStartMatch"));

	Empty_Params params;

	ProcessEvent(PlayerControllerFinder.GetObj(), fn, &params);
	PLOGI << "Server is ready to start match";
}

void UFunctions::SetPlaylist()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
	ObjectFinder WorldFinder = GameViewPortClientFinder.Find(XOR(L"World"));
	ObjectFinder GameStateFinder = WorldFinder.Find(XOR(L"GameState"));

	auto CurrentPlaylistInfoOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortGameStateAthena"), XOR(L"CurrentPlaylistInfo"));

	auto CurrentPlaylistInfo = reinterpret_cast<FPlaylistPropertyArray*>(reinterpret_cast<uintptr_t>(GameStateFinder.GetObj()) + CurrentPlaylistInfoOffset);

	CurrentPlaylistInfo->BasePlaylist = gPlaylist;
	CurrentPlaylistInfo->OverridePlaylist = gPlaylist;

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortGameStateAthena:OnRep_CurrentPlaylistInfo"));

	Empty_Params params;

	ProcessEvent(GameStateFinder.GetObj(), fn, &params);

	PLOGD << "Playlist was set";
}

void UFunctions::SetGamePhase()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
	ObjectFinder WorldFinder = GameViewPortClientFinder.Find(XOR(L"World"));
	ObjectFinder GameStateFinder = WorldFinder.Find(XOR(L"GameState"));

	auto GamePhaseOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortGameStateAthena"), XOR(L"GamePhase"));

	EAthenaGamePhase* GamePhase = reinterpret_cast<EAthenaGamePhase*>(reinterpret_cast<uintptr_t>(GameStateFinder.GetObj()) + GamePhaseOffset);

	*GamePhase = EAthenaGamePhase::None;

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortGameStateAthena:OnRep_GamePhase"));

	AFortGameStateAthena_OnRep_GamePhase_Params params;
	params.OldGamePhase = EAthenaGamePhase::Setup;

	ProcessEvent(GameStateFinder.GetObj(), fn, &params);

	PLOGD << "Game phase was set";
}

void UFunctions::LoadAndStreamInLevel(const wchar_t* EventSequenceMap)
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
	ObjectFinder WorldFinder = GameViewPortClientFinder.Find(XOR(L"World"));
	ObjectFinder NetworkManagerFinder = WorldFinder.Find(XOR(L"NetworkManager"));
	ObjectFinder PersistentLevelFinder = WorldFinder.Find(XOR(L"PersistentLevel"));

	//Loading the level instance in memory
	auto LoadLevelInstance = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.LevelStreamingDynamic:LoadLevelInstance"));
	auto LevelStreamingDynamic = UE4::FindObject<UObject*>(XOR(L"LevelStreamingDynamic /Script/Engine.Default__LevelStreamingDynamic"));

	FRotator WorldRotation;
	WorldRotation.Yaw = 0;
	WorldRotation.Roll = 0;
	WorldRotation.Pitch = 0;

	ULevelStreamingDynamic_LoadLevelInstance_Params LoadLevelInstanceParams;
	LoadLevelInstanceParams.WorldContextObject = WorldFinder.GetObj();
	LoadLevelInstanceParams.LevelName = EventSequenceMap;
	LoadLevelInstanceParams.Location = FVector(0, 0, 0);
	LoadLevelInstanceParams.Rotation = WorldRotation;

	ProcessEvent(LevelStreamingDynamic, LoadLevelInstance, &LoadLevelInstanceParams);

	ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));
	ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

	auto KismetSysLib = UE4::FindObject<UObject*>(XOR(L"KismetSystemLibrary /Script/Engine.Default__KismetSystemLibrary"));
	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.KismetSystemLibrary:ExecuteConsoleCommand"));

	std::wstring command = L"streamlevelin " + std::wstring(EventSequenceMap);

	UKismetSystemLibrary_ExecuteConsoleCommand_Params params;
	params.WorldContextObject = WorldFinder.GetObj();
	params.Command = command.c_str();
	params.SpecificPlayer = PlayerControllerFinder.GetObj();

	ProcessEvent(KismetSysLib, fn, &params);
}

void UFunctions::Play(const wchar_t* AnimationPlayerFullName)
{
	auto Play = UE4::FindObject<UFunction*>(XOR(L"Function /Script/MovieScene.MovieSceneSequencePlayer:Play"));

	auto Sequence = UE4::FindObject<void*>(AnimationPlayerFullName);

	ProcessEvent(Sequence, Play, nullptr);
}

void UFunctions::ConsoleLog(std::wstring message)
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
	ObjectFinder WorldFinder = GameViewPortClientFinder.Find(XOR(L"World"));
	ObjectFinder GameModeFinder = WorldFinder.Find(XOR(L"AuthorityGameMode"));

	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.GameMode:Say"));

	const FString Msg = message.c_str();
	AGameMode_Say_Params params;
	params.Msg = Msg;

	ProcessEvent(GameModeFinder.GetObj(), fn, &params);
}

void UFunctions::DestoryActor(UObject* actor)
{
	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Actor:K2_DestroyActor"));

	ProcessEvent(actor, fn, nullptr);
}

void UFunctions::PlayCustomPlayPhaseAlert()
{
	// ToDo: this shit is not working
	return;

	auto AGPCW = UE4::FindObject<UObject*>(XOR(L"AthenaGamePhaseChangeWidget_C /Engine/Transient.FortEngine_"));

	auto AGPCWFinder = ObjectFinder::EntryPoint(uintptr_t(AGPCW));

	// this things causes an error because UE4::FindObject<UObject *>() line 199 ->  UE4::GetObjectFullName() line 135 fails and crashes the game
	auto PlayIntroAnim = UE4::FindObject<UObject*>(XOR(L"Function /Game/Athena/HUD/Phase/AthenaGamePhaseChangeWidget.AthenaGamePhaseChangeWidget_C:PlayIntroAnimation"));

	PlayIntroAnim_Params PlayIntroAnimParams;

	PlayIntroAnimParams.Step = EAthenaGamePhaseStep::Count;

	ProcessEvent(AGPCW, PlayIntroAnim, &PlayIntroAnimParams);
}

auto UFunctions::StaticLoadObjectEasy(UClass* inClass, const wchar_t* inName, UObject* inOuter = nullptr)
{
	return StaticLoadObject(inClass, inOuter, inName, nullptr, 0, nullptr, false, nullptr);
}