#include "pch.h"
#include "Match.h"

#include "Functions.h"
#include "Console.h"
#include "FortPawn.h"

void Match::Start(const wchar_t* MapToPlayOn)
{
	MatchState = EMatchState::Loading;
	UFunctions::Travel(MapToPlayOn);
	bIsStarted = !bIsStarted;
}

void Match::Stop()
{
	UFunctions::Travel(XOR(L"Frontend?game=/Script/FortniteGame.FortGameModeFrontEnd"));
	bIsStarted = false;
	bIsInit = false;
	NeoPlayer.Pawn = nullptr;
	NeoPlayer.Mesh = nullptr;
	NeoPlayer.AnimInstance = nullptr;
	gPlaylist = nullptr;
	MatchState = EMatchState::InLobby;
}

void Match::LoadMoreClasses()
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

	auto bHasServerFinishedLoading = reinterpret_cast<bool*>(reinterpret_cast<uintptr_t>(GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject) + HasServerFinishedLoadingOffset);

	if (Util::IsBadReadPtr(bHasServerFinishedLoading))
	{
		PLOGE << "bHasServerFinishedLoading is null";
	}
	else
	{
		*bHasServerFinishedLoading = true;

		auto bHasClientFinishedLoadingOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortPlayerController"), XOR(L"bHasClientFinishedLoading"));

		auto bHasClientFinishedLoading = reinterpret_cast<bool*>(reinterpret_cast<uintptr_t>(GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject) + bHasClientFinishedLoadingOffset);

		if (!Util::IsBadReadPtr(bHasClientFinishedLoading)) {
			bool HasFinishedLoading = true;

			*bHasClientFinishedLoading = true;
		}
		else {
			PLOGE << "bHasClientFinishedLoading is null";
		}
	}
}

void Match::InitCombos()
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

void Match::Thread()
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

		if (GMatch.bWantsToJump)
		{
			// ToDo: add jump function
			GMatch.bWantsToJump = false;
		}

		else if (GMatch.bWantsToOpenGlider)
		{
			GMatch.NeoPlayer.ForceOpenParachute();
			GMatch.bWantsToOpenGlider = false;
		}

		else if (GMatch.bWantsToSkydive)
		{
			GMatch.NeoPlayer.Skydive();
			GMatch.bWantsToSkydive = false;
		}

		else if (GMatch.bWantsToShowPickaxe)
		{
			GMatch.NeoPlayer.ShowPickaxe();
			GMatch.bWantsToShowPickaxe = false;
		}

		Sleep(1000 / 30);
	}
}

DWORD Match::ThreadEntry(LPVOID* param)
{
	Match* myObj = (Match*)param;
	myObj->Thread();
	return 0;
}

void Match::Init()
{
	PLOGI << "Init Match";

	NeoPlayer.Setup();

	Console::CheatManager();

	NeoPlayer.Summon(XOR(L"PlayerPawn_Athena_C"));

	NeoPlayer.Pawn = ObjectFinder::FindActor(XOR(L"PlayerPawn_Athena_C"));

	if (Util::IsBadReadPtr(NeoPlayer.Pawn)) {
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

		CreateThread(nullptr, NULL, reinterpret_cast<LPTHREAD_START_ROUTINE>(*ThreadEntry), this, NULL, nullptr);

		bIsInit = !bIsInit;
	}

	PLOGI << "Finished Init Match";
}
