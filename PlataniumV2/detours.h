#pragma once
#include "ue4.h"
#include <minwindef.h>
#include "neoroyale.h"
#include "ue4.h"
#include "player.h"
#include <thread>

#ifndef PROD
//#define LOGGING
#endif

using namespace NeoRoyale;

inline bool bIsDebugCamera;
inline bool bIsFlying;

inline void LogThread (std::wstring nObj, std::wstring nFunc, std::wstring nObjClass) {
	PLOGV.printf(XOR("[Object]: %ws [Function]: %ws [Class]: %ws\n"), nObj.c_str(), nFunc.c_str(), nObjClass.c_str());
}

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

	else if (wcsstr(nFunc.c_str(), XOR(L"ReadyToStartMatch")) && bIsStarted && !bIsInit)
	{
		PLOGI << XOR("ReadyToStartMatch called");
		Init();
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"ServerLoadingScreenDropped")) && bIsInit && bIsStarted)
	{
		PLOGD << "ServerLoadingScreenDropped called";

		UFunctions::PlayCustomPlayPhaseAlert();

		LoadMoreClasses();
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"SetRenderingAPI")))
	{
		return nullptr;
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"SetFullscreenMode")))
	{
		return nullptr;
	}

	// Fly Command
	else if (wcsstr(nFunc.c_str(), XOR(L"Fly")) && nObj.starts_with(XOR(L"CheatManager_")))
	{
		NeoPlayer.Fly(bIsFlying);
		bIsFlying = !bIsFlying;
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"ServerAttemptAircraftJump")))
	{
		NeoPlayer.ExecuteConsoleCommand(XOR(L"PAUSESAFEZONE"));
		NeoPlayer.Respawn();
		auto currentLocation = NeoPlayer.GetLocation();
		UFunctions::TeleportToCoords(currentLocation.X, currentLocation.Y, currentLocation.Z);
	}

	// Interact with a specific object (for example open/close door)
	// This currently doesn't really work
	else if (wcsstr(nFunc.c_str(), XOR(L"ServerAttemptInteract")))
	{
		struct ServerAttemptInteract
		{
			UObject* ReceivingActor;
			UObject* InteractComponent;
			byte InteractType;
			struct UObject* OptionalObjectData;
			EInteractionBeingAttempted InteractionBeingAttempted;
			int32_t RequestID;
		};

		auto CurrentParams = (ServerAttemptInteract*)pParams;
		
		// ToDo: names are invalid
		if (!Util::IsBadReadPtr(CurrentParams->ReceivingActor)) {
			PLOGI.printf("Player wants to interact with %s", UE4::GetObjectFullName(CurrentParams->ReceivingActor).c_str());

			if (!Util::IsBadReadPtr(CurrentParams->ReceivingActor->Class)) {
				PLOGI.printf("Interactable object Class: %s", UE4::GetObjectFullName(CurrentParams->ReceivingActor->Class).c_str());
			}
		}
		else {
			PLOGE << "ServerAttemptInteract: CurrentParams->ReceivingActor is nullptr";
		}

		if (!Util::IsBadReadPtr(CurrentParams->InteractComponent)) {
			PLOGI.printf("InteractComponent: %s", UE4::GetObjectFullName(CurrentParams->InteractComponent).c_str());

			if (!Util::IsBadReadPtr(CurrentParams->InteractComponent->Class)) {
				PLOGI.printf("InteractComponent Class: %s", UE4::GetObjectFullName(CurrentParams->InteractComponent->Class).c_str());
			}
		}
		else {
			PLOGE << "ServerAttemptInteract: CurrentParams->InteractComponent is nullptr";
		}
	}

	else if (bIsInit)
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

	else if (wcsstr(nFunc.c_str(), XOR(L"EnableCheats")))
	{
		Console::CheatManager();
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"OnWeaponEquipped")))
	{
		auto params = static_cast<AFortPawn_OnWeaponEquipped_Params*>(pParams);

		auto OldWeapon = params->PrevWeapon;

		if (OldWeapon && !Util::IsBadReadPtr(OldWeapon))
		{
			UFunctions::DestoryActor(OldWeapon);
			OldWeapon = nullptr;
		}
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"BP_OnDeactivated")) && wcsstr(nObj.c_str(), XOR(L"PickerOverlay_EmoteWheel")))
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
			}
		}
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"BlueprintOnInteract")) && nObj.starts_with(XOR(L"BGA_FireExtinguisher_Pickup_C_")))
	{
		NeoPlayer.EquipWeapon(XOR(L"WID_FireExtinguisher_Spray"));
	}


	else if (wcsstr(nFunc.c_str(), XOR(L"CheatScript")))
	{
		FString ScriptNameF = static_cast<UCheatManager_CheatScript_Params*>(pParams)->ScriptName;

		PLOGI.printf("Called script %s", ScriptNameF.ToWString());

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
				break;
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
				break;
			}

			default:
				break;
			}
		}
	}

	//Logging
	if (true) {
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
			!wcsstr(nFunc.c_str(), L"OnHover") &&
			!wcsstr(nFunc.c_str(), L"OnHovered") &&
			!wcsstr(nFunc.c_str(), L"OnUnhover") &&
			!wcsstr(nFunc.c_str(), L"OnUnhovered") &&
			!wcsstr(nFunc.c_str(), L"HandleButtonReleased") &&
			!wcsstr(nFunc.c_str(), L"HandleButtonClicked") &&
			!wcsstr(nFunc.c_str(), L"ScrollNextItem") &&
			!wcsstr(nFunc.c_str(), L"OnCurrentTextStyleChanged") &&
			!wcsstr(nFunc.c_str(), L"OnButtonUnhovered") &&
			!wcsstr(nFunc.c_str(), L"OnButtonHovered") &&
			!wcsstr(nFunc.c_str(), L"OnRemovedFromFocusPath") &&
			!wcsstr(nFunc.c_str(), L"OnFocusLost") &&
			!wcsstr(nFunc.c_str(), L"OnUpdateNameplateVis") &&
			!wcsstr(nFunc.c_str(), L"/Script/UMG.Border.SetBrushColor") &&
			!wcsstr(nFunc.c_str(), L"AthenaMOTDTeaserWidget.AthenaMOTDTeaserWidget_C.HandleEntryWidgetHoveredChanged") &&
			!wcsstr(nFunc.c_str(), L"/Script/UMG.UserWidget.Destruct") &&

			// Camera
			!wcsstr(nFunc.c_str(), L"OnFrontEndCameraChanged") &&
			!wcsstr(nFunc.c_str(), L"FrontEndCameraSwitchFadeAthena__UpdateFunc") &&

			// Loading stuff
			!wcsstr(nFunc.c_str(), L"OnBuildingActorInitialized") &&
			!wcsstr(nFunc.c_str(), L"OnReady") &&
			!wcsstr(nFunc.c_str(), L"ReceiveBeginPlay") &&
			!wcsstr(nFunc.c_str(), L"Construct") &&
			!wcsstr(nFunc.c_str(), L"FortClientSettingsRecord") &&

			// ingame
			!wcsstr(nFunc.c_str(), L"BlueprintGetInteractionTime") &&
			!wcsstr(nFunc.c_str(), L"BGA_IslandPortal_C.CheckShouldDisplayUI") &&
			!wcsstr(nFunc.c_str(), L"PortalInfoPlate_C.OnUpdateNameplateVis") &&
			!wcsstr(nFunc.c_str(), L"FlopperSpawn") &&
			!wcsstr(nFunc.c_str(), L"SetRuntimeStats") &&
			!wcsstr(nFunc.c_str(), L"HandleSimulatingComponentHit") &&
			!wcsstr(nFunc.c_str(), L"ReceiveHit") &&
			!wcsstr(nFunc.c_str(), L"BGA_SuperSilkyWolf_C") &&
			!wcsstr(nFunc.c_str(), L"ServerFireAIDirectorEvent") &&
			!wcsstr(nFunc.c_str(), L"AnimNotify_FootStep") &&
			!wcsstr(nFunc.c_str(), L"ReceiveDestroyed") &&
			!wcsstr(nFunc.c_str(), L"AnimNotify") &&

			// Player
			!wcsstr(nFunc.c_str(), L"/Script/Engine.Character.CanJumpInternal") &&

			// Interaction
			!wcsstr(nFunc.c_str(), L"BlueprintCanInteract") &&
			!wcsstr(nFunc.c_str(), L"BlueprintGetInteractionString") &&

			//ingame ui
			!wcsstr(nFunc.c_str(), L"PopupCenterMessageWidget_C.UpdateStateEvent") &&
			!wcsstr(nFunc.c_str(), L"FortInteractInterface.GetFocusedSocketLocation") &&

			// BattlePass UI

			// Called when a new item is sliding in
			!wcsstr(nFunc.c_str(), L"BattlePassVaultWorld_C.Floor-Visibility__UpdateFunc") &&

			// ToDo: I think this gets called if it shows a music pack, but music is muted
			!wcsstr(nFunc.c_str(), L"B_MusicPackPreviewDisplay_C.UpdateMuteSetting") &&

			// Shutdown
			!wcsstr(nFunc.c_str(), L"/Script/Engine.ActorComponent.ReceiveEndPlay") &&

			!wcsstr(nFunc.c_str(), L"ReadyToEndMatch"))
		{
			std::thread log(LogThread, nObj, nFunc, UE4::GetObjectFullName(static_cast<UObject*>(pObj)->Class));
			log.detach();
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
