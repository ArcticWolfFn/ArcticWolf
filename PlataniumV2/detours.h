#pragma once
#include "ue4.h"
#include <minwindef.h>
#include "neoroyale.h"
#include "ue4.h"
#include "player.h"
//#include "hwid.h"
//#include "kismet.h"

#ifndef PROD
//#define LOGGING
#endif

using namespace NeoRoyale;

inline bool bIsDebugCamera;
inline bool bIsFlying;

inline void* ProcessEventDetour(UObject* pObj, UObject* pFunc, void* pParams)
{
	auto nObj = UE4::GetObjectName(pObj);
	auto nFunc = UE4::GetObjectName(pFunc);


	//If the game requested matchmaking we open the game mode
	if (wcsstr(nFunc.c_str(), XOR(L"OnSetPlayButtonText")) && wcsstr(nObj.c_str(), XOR(L"Matchmaking_AthenaLegacy")) && !bIsStarted)
	{
		PLOGI << XOR("[NeoRoyale] Start!");

		auto Playlist = UE4::FindObject<UObject*>(XOR(L"FortPlaylistAthena /Game/Athena/Playlists/BattleLab/Playlist_BattleLab.Playlist_BattleLab"));
		gPlaylist = Playlist;
		auto Map = APOLLO_TERRAIN;

		Start(Map);
	}

	
	if (wcsstr(nFunc.c_str(), XOR(L"ReadyToStartMatch")) && bIsStarted && !bIsInit)
	{
		PLOGI << XOR("ReadyToStartMatch called");
		Init();
	}

	if (wcsstr(nFunc.c_str(), XOR(L"DynamicHandleLoadingScreenVisibilityChanged")) && wcsstr(nObj.c_str(), XOR(L"AthenaLobby")))
	{
		PLOGD << "DynamicHandleLoadingScreenVisibilityChanged called";
		if (bIsDebugCamera) bIsDebugCamera = !bIsDebugCamera;
		UFunctions::RegionCheck();
	}

	if (wcsstr(nFunc.c_str(), XOR(L"ServerLoadingScreenDropped")) && bIsInit && bIsStarted)
	{
		PLOGD << "ServerLoadingScreenDropped called";
		/*if (gVersion > 14.30f)
		{*/
			// disabled because it crashes the game
			// UFunctions::SetupCustomInventory();
		//}

		UFunctions::PlayCustomPlayPhaseAlert();
		// LoadMoreClasses();
	}

	if (wcsstr(nFunc.c_str(), XOR(L"SetRenderingAPI")))
	{
		return nullptr;
	}

	if (wcsstr(nFunc.c_str(), XOR(L"SetFullscreenMode")))
	{
		return nullptr;
	}

	//Toggle our fly function on "fly" command.
	if (wcsstr(nFunc.c_str(), XOR(L"Fly")) && nObj.starts_with(XOR(L"CheatManager_")))
	{
		NeoPlayer.Fly(bIsFlying);
		bIsFlying = !bIsFlying;
	}

	// NOTE: (irma) This is better.
	if (wcsstr(nFunc.c_str(), XOR(L"ServerAttemptAircraftJump")))
	{
		NeoPlayer.ExecuteConsoleCommand(XOR(L"PAUSESAFEZONE"));
		NeoPlayer.Respawn();
		auto currentLocation = NeoPlayer.GetLocation();
		UFunctions::TeleportToCoords(currentLocation.X, currentLocation.Y, currentLocation.Z);
	}

	if (bIsInit)
	{
		if (bWantsToJump)
		{
			NeoPlayer.Jump();
			bWantsToJump = false;
		}

		else if (bWantsToOpenGlider)
		{
			NeoPlayer.ForceOpenParachute();
			bWantsToOpenGlider = false;
		}

		else if (bWantsToSkydive)
		{
			NeoPlayer.Skydive();
			bWantsToSkydive = false;
		}

		else if (bWantsToShowPickaxe)
		{
			NeoPlayer.ShowPickaxe();
			bWantsToShowPickaxe = false;
		}
	}

	if (wcsstr(nFunc.c_str(), XOR(L"EnableCheats")))
	{
		Console::CheatManager();
	}

	if (wcsstr(nFunc.c_str(), XOR(L"OnWeaponEquipped")))
	{
		auto params = static_cast<AFortPawn_OnWeaponEquipped_Params*>(pParams);

		auto OldWeapon = params->PrevWeapon;

		if (OldWeapon && !Util::IsBadReadPtr(OldWeapon))
		{
			UFunctions::DestoryActor(OldWeapon);
			OldWeapon = nullptr;
		}
	}

	if (wcsstr(nFunc.c_str(), XOR(L"BP_OnDeactivated")) && wcsstr(nObj.c_str(), XOR(L"PickerOverlay_EmoteWheel")))
	{
		if (NeoPlayer.Pawn)
		{
			ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
			ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

			ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

			ObjectFinder LastEmotePlayedFinder = PlayerControllerFinder.Find(XOR(L"LastEmotePlayed"));

			auto LastEmotePlayed = LastEmotePlayedFinder.GetObj();

			if (LastEmotePlayed && !Util::IsBadReadPtr(LastEmotePlayed))
			{
				NeoPlayer.Emote(LastEmotePlayed);
				for (auto i = 0; i < Bots.size(); i++)
				{
					auto Bot = Bots[i];
					if (Bot.Pawn)
					{
						Bot.Emote(LastEmotePlayed);
					}
				}
			}
		}
	}

	if (wcsstr(nFunc.c_str(), XOR(L"ReceiveHit")) && nObj.starts_with(XOR(L"Prj_Athena_FrenchYedoc_JWFriendly_C")))
	{
		Player Bot;
		const auto Params = static_cast<AActor_ReceiveHit_Params*>(pParams);
		auto HitLocation = Params->HitLocation;

		NeoPlayer.Summon(XOR(L"BP_PlayerPawn_Athena_Phoebe_C"));
		Bot.Pawn = ObjectFinder::FindActor(XOR(L"BP_PlayerPawn_Athena_Phoebe_C"), Bots.size());

		if (Bot.Pawn)
		{
			HitLocation.Z = HitLocation.Z + 200;

			FRotator Rotation;
			Rotation.Yaw = 0;
			Rotation.Roll = 0;
			Rotation.Pitch = 0;

			Bot.TeleportTo(HitLocation, Rotation);

			Bot.SetSkeletalMesh(XOR(L"SK_M_MALE_Base"));
			Bot.Emote(UE4::FindObject<UObject*>(XOR(L"EID_HightowerSquash.EID_HightowerSquash"), true));

			Bots.push_back(Bot);
		}
	}

	if (wcsstr(nFunc.c_str(), XOR(L"BlueprintOnInteract")) && nObj.starts_with(XOR(L"BGA_FireExtinguisher_Pickup_C_")))
	{
		NeoPlayer.EquipWeapon(XOR(L"WID_FireExtinguisher_Spray"));
	}


	if (wcsstr(nFunc.c_str(), XOR(L"CheatScript")))
	{
		FString ScriptNameF = static_cast<UCheatManager_CheatScript_Params*>(pParams)->ScriptName;

		if (ScriptNameF.IsValid())
		{
			std::wstring ScriptNameW = ScriptNameF.ToWString();

			std::wstring arg;

			if (ScriptNameW.find(L" ") != std::wstring::npos)
			{
				arg = ScriptNameW.substr(ScriptNameW.find(L" ") + 1);
			}

			auto CMD = str2enum(ScriptNameW.c_str());

			switch (CMD)
			{
			case HELP:
			{
				UFunctions::ConsoleLog(CheatScriptHelp);
				break;
			}

			case TEST:
			{
				break;
			}

			case DUMP:
			{
				UE4::DumpGObjects();
				break;
			}

			case DUMPBPS:
			{
				UE4::DumpBPs();
				break;
			}
			case EVENT:
			{
				/*if (gVersion == 14.60f)
				{
					UFunctions::Play(GALACTUS_EVENT_PLAYER);
				}
				else if (gVersion == 12.41f)
				{
					UFunctions::Play(JERKY_EVENT_PLAYER);
				}
				else if (gVersion == 12.61f)
				{
					UFunctions::Play(DEVICE_EVENT_PLAYER);
				}
				else
				{*/
					UFunctions::ConsoleLog(XOR(L"Sorry, events are currently not supported."));
				//}
				break;
			}

			case DEBUG_CAMERA:
			{
				bIsDebugCamera = !bIsDebugCamera;
				break;
			}

			case EQUIP:
			{
				if (!arg.empty())
				{
					NeoPlayer.EquipWeapon(arg.c_str());
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument"));
				}
				break;
			}

			case SET_MAX_HEALTH:
			{
				if (!arg.empty())
				{
					auto n = std::stof(arg);
					NeoPlayer.SetMaxHealth(n);
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument, e.g: (cheatscript setmaxhealth 1000)"));
				}
				break;
			}

			case SET_MAX_SHIELD:
			{
				if (!arg.empty())
				{
					auto n = std::stof(arg);
					NeoPlayer.SetMaxShield(n);
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument e.g: (cheatscript setmaxshield 1000)"));
				}
				break;
			}

			case SET_HEALTH:
			{
				if (!arg.empty())
				{
					auto n = std::stof(arg);
					NeoPlayer.SetHealth(n);
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument e.g: (cheatscript sethealth 1000)"));
				}
				break;
			}

			case SET_SHIELD:
			{
				if (!arg.empty())
				{
					auto n = std::stof(arg);
					NeoPlayer.SetShield(n);
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument e.g: (cheatscript setshiled 1000)"));
				}
				break;
			}

			case SET_SPEED:
			{
				if (!arg.empty())
				{
					auto n = std::stof(arg);
					NeoPlayer.SetMovementSpeed(n);
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument e.g: (cheatscript setspeed 1000)"));
				}
				break;
			}

			case SET_GRAVITY:
			{
				if (!arg.empty())
				{
					auto n = std::stof(arg);
					NeoPlayer.SetPawnGravityScale(n);
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument"));
				}
				break;
			}

			case SET_PLAYLIST:
			{
				if (!arg.empty())
				{
					auto Playlist = UE4::FindObject<UObject*>(ScriptNameW.c_str());
					if (Playlist)
					{
						gPlaylist = Playlist;
					}
					else
					{
						UFunctions::ConsoleLog(XOR(L"Couldn't find the requested playlist!."));
					}
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument"));
				}
				break;
			}

			case SKYDIVE:
			{
				NeoPlayer.StartSkydiving(0);
				NeoPlayer.StartSkydiving(0);
				NeoPlayer.StartSkydiving(0);
				NeoPlayer.StartSkydiving(1500.0f);
				break;
			}

			case RESPAWN:
			{
				NeoPlayer.Respawn();
			}

			case LOADBPC:
			{
				if (!arg.empty())
				{
					const auto BPGClass = UE4::FindObject<UClass*>(XOR(L"Class /Script/Engine.BlueprintGeneratedClass"));

					UFunctions::StaticLoadObjectEasy(BPGClass, arg.c_str());
				}
				else
				{
					UFunctions::ConsoleLog(XOR(L"This command requires an argument"));
				}
			}

			default:
				break;
			}
		}
	}

	//Logging
	if (false) {
		if (!wcsstr(nFunc.c_str(), L"EvaluateGraphExposedInputs") &&
			!wcsstr(nFunc.c_str(), L"Tick") &&
			!wcsstr(nFunc.c_str(), L"OnSubmixEnvelope") &&
			!wcsstr(nFunc.c_str(), L"OnSubmixSpectralAnalysis") &&
			!wcsstr(nFunc.c_str(), L"OnMouse") &&
			!wcsstr(nFunc.c_str(), L"Pulse") &&
			!wcsstr(nFunc.c_str(), L"BlueprintUpdateAnimation") &&
			!wcsstr(nFunc.c_str(), L"BlueprintPostEvaluateAnimation") &&
			!wcsstr(nFunc.c_str(), L"BlueprintModifyCamera") &&
			!wcsstr(nFunc.c_str(), L"BlueprintModifyPostProcess") &&
			!wcsstr(nFunc.c_str(), L"Loop Animation Curve") &&
			!wcsstr(nFunc.c_str(), L"UpdateTime") &&
			!wcsstr(nFunc.c_str(), L"GetMutatorByClass") &&
			!wcsstr(nFunc.c_str(), L"UpdatePreviousPositionAndVelocity") &&
			!wcsstr(nFunc.c_str(), L"IsCachedIsProjectileWeapon") &&
			!wcsstr(nFunc.c_str(), L"LockOn") &&
			!wcsstr(nFunc.c_str(), L"GetAbilityTargetingLevel") &&
			!wcsstr(nFunc.c_str(), L"ServerTouchActiveTime") &&

			//UI
			!wcsstr(nFunc.c_str(), L"OnAnimationFinished") &&
			!wcsstr(nFunc.c_str(), L"OnAnimationStarted") &&
			!wcsstr(nFunc.c_str(), L"SetColorAndOpacity") &&
			!wcsstr(nFunc.c_str(), L"OnHovered") &&
			!wcsstr(nFunc.c_str(), L"OnUnhovered") &&
			!wcsstr(nFunc.c_str(), L"HandleButtonReleased") &&
			!wcsstr(nFunc.c_str(), L"HandleButtonClicked") &&
			!wcsstr(nFunc.c_str(), L"ScrollNextItem") &&
			!wcsstr(nFunc.c_str(), L"OnCurrentTextStyleChanged") &&
			!wcsstr(nFunc.c_str(), L"OnButtonUnhovered") &&
			!wcsstr(nFunc.c_str(), L"OnButtonHovered") &&

			!wcsstr(nFunc.c_str(), L"ReadyToEndMatch"))
		{
			PLOGV.printf(XOR("[Object]: %ws [Function]: %ws [Class]: %ws\n"), nObj.c_str(), nFunc.c_str(), UE4::GetObjectFullName(static_cast<UObject*>(pObj)->Class).c_str());
		}
	}

	return ProcessEvent(pObj, pFunc, pParams);
}

namespace CameraHook
{
	inline float Speed = 0.1;
	inline float FOV = 52.0;
	inline FVector Camera(52.274170, 125912.695313, 89.249969);
	inline FRotator Rotation = { 0.870931, -88.071960, 0.008899 };
}

inline int GetViewPointDetour(void* pPlayer, FMinimalViewInfo* pViewInfo, BYTE stereoPass)
{
	const auto CurrentViewPoint = GetViewPoint(pPlayer, pViewInfo, stereoPass);
	/*
	if (bIsDebugCamera)
	{
		//fov increase and decrease
		//if (GetAsyncKeyState(VK_OEM_PLUS) == 0) CameraHook::FOV += CameraHook::Speed;

		//if (GetAsyncKeyState(VK_OEM_MINUS) == 0) CameraHook::FOV -= CameraHook::Speed;

		//froward and backward left and right
		if (GetAsyncKeyState(0x57) == 0) CameraHook::Camera.Y += CameraHook::Speed;

		if (GetAsyncKeyState(0x53) == 0) CameraHook::Camera.Y -= CameraHook::Speed;

		if (GetAsyncKeyState(0x41) == 0) CameraHook::Camera.X += CameraHook::Speed;

		if (GetAsyncKeyState(0x44) == 0) CameraHook::Camera.X -= CameraHook::Speed;

		//up and down
		if (GetAsyncKeyState(0x45) == 0) CameraHook::Camera.Z += CameraHook::Speed;

		if (GetAsyncKeyState(0x51) == 0) CameraHook::Camera.Z -= CameraHook::Speed;


		//looking around
		if (GetAsyncKeyState(VK_UP) == 0) CameraHook::Rotation.Pitch -= CameraHook::Speed;

		if (GetAsyncKeyState(VK_DOWN) == 0) CameraHook::Rotation.Pitch += CameraHook::Speed;

		if (GetAsyncKeyState(VK_LEFT) == 0) CameraHook::Rotation.Yaw += CameraHook::Speed;

		if (GetAsyncKeyState(VK_RIGHT) == 0) CameraHook::Rotation.Yaw -= CameraHook::Speed;

		//assign our hooked variables
		pViewInfo->Location.X = CameraHook::Camera.X;
		pViewInfo->Location.Y = CameraHook::Camera.Y;
		pViewInfo->Location.Z = CameraHook::Camera.Z;

		pViewInfo->Rotation.Pitch = CameraHook::Rotation.Pitch;
		pViewInfo->Rotation.Yaw = CameraHook::Rotation.Yaw;
		pViewInfo->Rotation.Roll = CameraHook::Rotation.Roll;

		pViewInfo->FOV = CameraHook::FOV;
	}*/

	return CurrentViewPoint;
}
