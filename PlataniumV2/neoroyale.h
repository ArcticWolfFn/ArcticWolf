﻿#pragma once
#include "mods.h"
#include "sdk.h"
#include "mods.h"

inline std::vector<std::wstring> gWeapons;
inline std::vector<std::wstring> gBlueprints;
inline std::vector<std::wstring> gMeshes;

namespace NeoRoyale
{
	inline bool bIsInit;
	inline bool bIsStarted;
	inline bool bIsPlayerInit;

	inline bool bHasJumped;
	inline bool bHasShowedPickaxe;

	inline bool bWantsToJump;
	inline bool bWantsToSkydive;
	inline bool bWantsToOpenGlider;
	inline bool bWantsToShowPickaxe;

	inline Player NeoPlayer;

	inline void Start(const wchar_t* MapToPlayOn)
	{
		UFunctions::Travel(MapToPlayOn);
		bIsStarted = !bIsStarted;
	}

	inline void Stop()
	{
		UFunctions::Travel(FRONTEND);
		bIsStarted = false;
		bIsInit = false;
		NeoPlayer.Controller = nullptr;
		NeoPlayer.Pawn = nullptr;
		NeoPlayer.Mesh = nullptr;
		NeoPlayer.AnimInstance = nullptr;
		gPlaylist = nullptr;
	}

	inline void LoadMoreClasses()
	{
		const auto BPGClass = UE4::FindObject<UClass*>(XOR(L"Class /Script/Engine.BlueprintGeneratedClass"));

		//Mech
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Athena/DrivableVehicles/Mech/TestMechVehicle.TestMechVehicle_C"));

		//Husks
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Characters/Enemies/Husk/Blueprints/HuskPawn.HuskPawn_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Characters/Enemies/DudeBro/Blueprints/DUDEBRO_Pawn.DUDEBRO_Pawn_C"));


		//CameraFilters
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_FilmNoir.PP_FilmNoir_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_Crazy.PP_Crazy_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_Dark.PP_Dark_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_HappyPlace.PP_HappyPlace_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_Oak.PP_Oak_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_Pixelizer.PP_Pixelizer_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_Red.PP_Red_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_Retro.PP_Retro_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_Sepia.PP_Sepia_C"));
		UFunctions::StaticLoadObjectEasy(BPGClass, XOR(L"/Game/Creative/PostProcess/PP_Spooky.PP_Spooky_C"));

		// show main menu

		auto HasServerFinishedLoadingOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortPlayerController"), XOR(L"bHasServerFinishedLoading"));

		auto bHasServerFinishedLoading = reinterpret_cast<bool*>(reinterpret_cast<uintptr_t>(NeoPlayer.Controller) + HasServerFinishedLoadingOffset);

		if (Util::IsBadReadPtr(bHasServerFinishedLoading)) 
		{
			PLOGE << "bHasServerFinishedLoading is null";
		}
		else 
		{
			*bHasServerFinishedLoading = true;

			auto bHasClientFinishedLoadingOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortPlayerController"), XOR(L"bHasClientFinishedLoading"));

			auto bHasClientFinishedLoading = reinterpret_cast<bool*>(reinterpret_cast<uintptr_t>(NeoPlayer.Controller) + bHasClientFinishedLoadingOffset);

			if (!Util::IsBadReadPtr(bHasClientFinishedLoading)) {
				bool HasFinishedLoading = true;

				*bHasClientFinishedLoading = true;
			}
			else {
				PLOGE << "bHasClientFinishedLoading is null";
			}
		}

		NeoPlayer.Setup();
	}

	inline void InitCombos()
	{
		for (auto i = 0x0; i < GObjs->NumElements; ++i)
		{
			auto object = GObjs->GetByIndex(i);
			if (object == nullptr)
			{
				continue;
			}

			if (!Util::IsBadReadPtr(object))
			{
				auto objectFullName = UE4::GetObjectFullName(object);
				auto objectFirstName = UE4::GetObjectFirstName(object);

				if ((objectFullName.starts_with(L"AthenaGadget") || objectFirstName.starts_with(L"WID_")) && !objectFirstName.starts_with(L"Default__"))
				{
					gWeapons.push_back(objectFirstName);
				}
				else if (objectFirstName.ends_with(L"_C") && !objectFirstName.starts_with(L"Default__"))
				{
					gBlueprints.push_back(objectFirstName);
				}
				else if (objectFullName.starts_with(L"SkeletalMesh ") && !objectFirstName.starts_with(L"Default__"))
				{
					gMeshes.push_back(objectFirstName);
				}
			}
		}
	}

	inline void Thread()
	{
		PLOGD << "Game Thread started";

		//NOTE (kemo): i know this isn't the best practice but it does the job on another thread so it's not a frezzing call
		while (true)
		{
			if (NeoPlayer.Pawn && GetAsyncKeyState(VK_SPACE))
			{
				if (!bHasJumped)
				{
					bHasJumped = !bHasJumped;
					if (!NeoPlayer.IsInAircraft())
					{
						bool isSkydiving = NeoPlayer.IsSkydiving();

						if (isSkydiving) {
							bool isParachuteOpen = NeoPlayer.IsParachuteOpen();
							bool isParachuteForcedOpen = NeoPlayer.IsParachuteForcedOpen();

							if (!isParachuteOpen && !isParachuteForcedOpen)
							{
								bWantsToOpenGlider = true;
							}
							else if (isParachuteOpen && !isParachuteForcedOpen)
							{
								bWantsToSkydive = true;
							}
						}
						else if (!NeoPlayer.IsJumpProvidingForce())
						{
							PLOGD << "Player want to jump";
							bWantsToJump = true;
						}
					}
				}
			}
			else bHasJumped = false;


			if (NeoPlayer.Pawn && GetAsyncKeyState(0x31) /* 1 key */)
			{
				if (!NeoPlayer.IsInAircraft())
				{
					if (!bHasShowedPickaxe)
					{
						bHasShowedPickaxe = !bHasShowedPickaxe;
						NeoPlayer.StopMontageIfEmote();
						bWantsToShowPickaxe = true;
					}
				}
			}
			else bHasShowedPickaxe = false;


			if (NeoPlayer.Pawn && GetAsyncKeyState(VK_F3))
			{
				Stop();
				break;
			}

			Sleep(1000 / 30);
		}
	}

	inline void Init()
	{
		PLOGI << "Init Match";

		Console::CheatManager();

		UFunctions::DestroyAllHLODs();

		NeoPlayer.Summon(XOR(L"PlayerPawn_Athena_C"));

		NeoPlayer.Pawn = ObjectFinder::FindActor(XOR(L"PlayerPawn_Athena_C"));

		if (NeoPlayer.Pawn == nullptr) {
			PLOGE << "Player Pawn is null";
		}

		NeoPlayer.Authorize();

		if (NeoPlayer.Pawn)
		{
			NeoPlayer.Possess();

			NeoPlayer.ShowSkin();

			NeoPlayer.ShowPickaxe();

			NeoPlayer.ToggleInfiniteAmmo();

			//LOL
			NeoPlayer.ExecuteConsoleCommand(XOR(L"god"));
			NeoPlayer.SetMovementSpeed(1.1);

			auto PlaylistName = UE4::GetObjectFirstName(gPlaylist);

			if (!wcsstr(PlaylistName.c_str(), XOR(L"Playlist_Papaya")) &&
				!wcsstr(PlaylistName.c_str(), XOR(L"Playlist_BattleLab")))
			{
				UFunctions::TeleportToSpawn();
			}

			UFunctions::SetPlaylist();

			UFunctions::SetGamePhase();

			InitCombos();

			UFunctions::StartMatch();

			UFunctions::ServerReadyToStartMatch();

			CreateThread(nullptr, NULL, reinterpret_cast<LPTHREAD_START_ROUTINE>(&Thread), nullptr, NULL, nullptr);

			bIsInit = !bIsInit;
		}

		PLOGI << "Finished Init Match";
	}
}
