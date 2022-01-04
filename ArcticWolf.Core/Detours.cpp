#include "pch.h"
#include "Detours.h"
#include "Match.h"
#include "FortPlaylistAthena.h"
#include "Functions.h"
#include "Console.h"

enum class EInteractionBeingAttempted : uint8_t {
	FirstInteraction,
	SecondInteraction,
	AllInteraction,
	EInteractionBeingAttempted_MAX,
};

struct Internal_AFortPawn_OnWeaponEquipped_Params
{
	InternalUObject* NewWeapon;
	InternalUObject* PrevWeapon;
};

struct AFortPawn_OnWeaponEquipped_Params
{
	AActor* NewWeapon;
	AActor* PrevWeapon;
};

struct UCheatManager_CheatScript_Params
{
	FString ScriptName;
};

static const wchar_t* CheatScriptHelp =
LR"(
Custom Cheatscript Commands
---------------------------
cheatscript event - Triggers the event for your version (e.g. Junior, Jerky, NightNight).
cheatscript debugcamera - Toggles a custom version of the debug camera.
cheatscript skydive | skydiving - Puts you in a skydive with deploy at 500m above the ground.
cheatscript equip <WID | AGID> - Equips a weapon / pickaxe.
cheatscript setgravity <NewGravityScaleFloat> - Changes the gravity scale.
cheatscript speed | setspeed <NewCharacterSpeedMultiplier> - Changes the movement speed multiplier.
cheatscript setplaylist <Playlist> - Overrides the current playlist.
cheatscript respawn - Respawns the player (duh)
cheatscript sethealth <NewHealthFloat> - Changes your health value.
cheatscript setshield <NewShieldFloat> - Changes your shield value.
cheatscript setmaxhealth <NewMaxHealthFloat> - Changes your max health value.
cheatscript setmaxshield <newMaxShieldFloat> - Changes your max shield value.
cheatscript dump - Dumps a list of all GObjects.
cheatscript dumpbps - Dumps all blueprints.
fly - Toggles flying.
enablecheats - Enables cheatmanager.
)";

void Detours::Log(std::wstring nObj, std::wstring nFunc, std::wstring nObjClass)
{
	const wchar_t* nFunc2 = nFunc.c_str();

	for (const wchar_t* ignoredFunc : ignoredFunctionNames)
	{
		if (wcsstr(nFunc2, ignoredFunc)) {
			return;
		}
	}
	PLOGV.printf(XOR("[Object]: %ws [Function]: %ws [Class]: %ws\n"), nObj.c_str(), nFunc.c_str(), nObjClass.c_str());
}

enum ECommands
{
	HELP,
	ACTIVATE,
	EVENT,
	DEBUG_CAMERA,
	SKYDIVE,
	EQUIP,
	SET_GRAVITY,
	SET_SPEED,
	SET_PLAYLIST,
	RESPAWN,
	SET_HEALTH,
	SET_SHIELD,
	SET_MAX_HEALTH,
	SET_MAX_SHIELD,
	DUMP,
	DUMPBPS,
	TEST,
	LOADBPC,
	DESTROY_HLODSM,
	NONE
};

static auto str2enum(const std::wstring& str)
{
	if (str.starts_with(L"event")) return EVENT;
	else if (str.starts_with(L"help")) return HELP;
	else if (str.starts_with(L"activate")) return DEBUG_CAMERA;
	else if (str.starts_with(L"debugcamera")) return DEBUG_CAMERA;
	else if (str.starts_with(L"skydive")) return SKYDIVE;
	else if (str.starts_with(L"skydiving")) return SKYDIVE;
	else if (str.starts_with(L"equip")) return EQUIP;
	else if (str.starts_with(L"setgravity")) return SET_GRAVITY;
	else if (str.starts_with(L"setspeed") || str.starts_with(L"speed")) return SET_SPEED;
	else if (str.starts_with(L"setplaylist")) return SET_PLAYLIST;
	else if (str.starts_with(L"respawn")) return RESPAWN;
	else if (str.starts_with(L"sethealth")) return SET_HEALTH;
	else if (str.starts_with(L"setshield")) return SET_SHIELD;
	else if (str.starts_with(L"setmaxhealth")) return SET_MAX_HEALTH;
	else if (str.starts_with(L"setmaxshield")) return SET_MAX_SHIELD;
	else if (str.starts_with(L"dump")) return DUMP;
	else if (str.starts_with(L"test")) return TEST;
	else if (str.starts_with(L"dumpbps")) return DUMPBPS;
	else if (str.starts_with(L"loadbpc")) return LOADBPC;
	else if (str.starts_with(L"destroyhlodsm")) return DESTROY_HLODSM;
	else return NONE;
}

void* Detours::ProcessEventDetour(InternalUObject* pObj, InternalUObject* pFunc, void* pParams)
{
	auto nObj = UE4::GetObjectName(pObj);

	auto nFunc = UE4::GetObjectName(pFunc);

	if (GMatch.bIsInit)
	{
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
	}
	
	//If the game requested matchmaking we open the game mode
	if (wcsstr(nFunc.c_str(), XOR(L"HandleButtonClicked")) && wcsstr(nObj.c_str(), XOR(L"AthenaLobby.WidgetTree.Matchmaking_AthenaLegacy.WidgetTree.Button_Play")) && !GMatch.bIsStarted)
	{
		PLOGI << XOR("[NeoRoyale] Start!");

		auto Playlist = UE4::FindObject<InternalUObject*>(XOR(L"FortPlaylistAthena /Game/Athena/Playlists/BattleLab/Playlist_BattleLab.Playlist_BattleLab"));
		gPlaylist = Playlist;
		auto Map = XOR(L"Apollo_Terrain?game=/Game/Athena/Athena_GameMode.Athena_GameMode_C");

		GMatch.Start(Map);
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"ReadyToStartMatch")) && GMatch.bIsStarted && !GMatch.bIsInit)
	{
		PLOGI << XOR("ReadyToStartMatch called");
		GMatch.Init();
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"ServerLoadingScreenDropped")) && GMatch.bIsInit && GMatch.bIsStarted)
	{
		PLOGD << "ServerLoadingScreenDropped called";

		//UFunctions::PlayCustomPlayPhaseAlert();

		GMatch.LoadMoreClasses();
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
		GMatch.NeoPlayer.Fly(bIsFlying);
		bIsFlying = !bIsFlying;
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"ServerAttemptAircraftJump")))
	{
		GMatch.NeoPlayer.ExecuteConsoleCommand(XOR(L"PAUSESAFEZONE"));
		GMatch.NeoPlayer.Respawn();
		auto currentLocation = GMatch.NeoPlayer.GetLocation();
		UFunctions::TeleportToCoords(currentLocation.X, currentLocation.Y, currentLocation.Z);
	}

	// Interact with a specific object (for example open/close door)
	// This currently doesn't really work
	else if (wcsstr(nFunc.c_str(), XOR(L"ServerAttemptInteract")))
	{
		struct ServerAttemptInteract
		{
			InternalUObject* ReceivingActor;
			InternalUObject* InteractComponent;
			std::byte InteractType;
			InternalUObject* OptionalObjectData;
			EInteractionBeingAttempted InteractionBeingAttempted;
			int32_t RequestID;
		};

		auto CurrentParams = (ServerAttemptInteract*)pParams;

		if (!Util::IsBadReadPtr(CurrentParams->ReceivingActor)) {
			PLOGI.printf("Player wants to interact with %ws", UE4::GetObjectFullName(CurrentParams->ReceivingActor).c_str());

			if (!Util::IsBadReadPtr(CurrentParams->ReceivingActor->Class)) {
				PLOGI.printf("Interactable object Class: %ws", UE4::GetObjectFullName(CurrentParams->ReceivingActor->Class).c_str());
			}
		}
		else {
			PLOGE << "ServerAttemptInteract: CurrentParams->ReceivingActor is nullptr";
		}

		if (!Util::IsBadReadPtr(CurrentParams->InteractComponent)) {
			PLOGI.printf("InteractComponent: %ws", UE4::GetObjectFullName(CurrentParams->InteractComponent).c_str());

			if (!Util::IsBadReadPtr(CurrentParams->InteractComponent->Class)) {
				PLOGI.printf("InteractComponent Class: %ws", UE4::GetObjectFullName(CurrentParams->InteractComponent->Class).c_str());
			}
		}
		else {
			PLOGE << "ServerAttemptInteract: CurrentParams->InteractComponent is nullptr";
		}
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"EnableCheats")))
	{
		Console::CheatManager();
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"OnWeaponEquipped")))
	{
		auto params = static_cast<Internal_AFortPawn_OnWeaponEquipped_Params*>(pParams);

		auto convertedParams = AFortPawn_OnWeaponEquipped_Params();
		convertedParams.NewWeapon = new AActor(params->NewWeapon);
		convertedParams.PrevWeapon = new AActor(params->PrevWeapon);

		auto OldWeapon = convertedParams.PrevWeapon;

		if (OldWeapon && !Util::IsBadReadPtr(params->PrevWeapon))
		{
			UFunctions::DestoryActor(OldWeapon);
			OldWeapon = nullptr;
		}
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"BP_OnDeactivated")) && wcsstr(nObj.c_str(), XOR(L"PickerOverlay_EmoteWheel")))
	{
		if (GMatch.NeoPlayer.Pawn)
		{
			ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
			ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

			ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

			ObjectFinder LastEmotePlayedFinder = PlayerControllerFinder.Find(XOR(L"LastEmotePlayed"));

			auto LastEmotePlayed = LastEmotePlayedFinder.GetObj();

			if (LastEmotePlayed && !Util::IsBadReadPtr(LastEmotePlayed))
			{
				GMatch.NeoPlayer.Emote(LastEmotePlayed);
			}
		}
	}

	else if (wcsstr(nFunc.c_str(), XOR(L"BlueprintOnInteract")) && nObj.starts_with(XOR(L"BGA_FireExtinguisher_Pickup_C_")))
	{
		GMatch.NeoPlayer.EquipWeapon(XOR(L"WID_FireExtinguisher_Spray"));
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
					GMatch.NeoPlayer.EquipWeapon(arg.c_str());
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
					GMatch.NeoPlayer.SetMaxHealth(n);
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
					GMatch.NeoPlayer.SetMaxShield(n);
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
					GMatch.NeoPlayer.SetHealth(n);
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
					GMatch.NeoPlayer.SetShield(n);
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
					GMatch.NeoPlayer.SetMovementSpeed(n);
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
					GMatch.NeoPlayer.SetPawnGravityScale(n);
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
					auto Playlist = UE4::FindObject<InternalUObject*>(ScriptNameW.c_str());
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
				GMatch.NeoPlayer.StartSkydiving(0);
				GMatch.NeoPlayer.StartSkydiving(0);
				GMatch.NeoPlayer.StartSkydiving(0);
				GMatch.NeoPlayer.StartSkydiving(1500.0f);
				break;
			}

			case RESPAWN:
			{
				GMatch.NeoPlayer.Respawn();
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

			case DESTROY_HLODSM:
			{
				UFunctions::DestroyAllHLODs();
				UFunctions::ConsoleLog(XOR(L"Destroyed all HLODs!"));
			}

			default:
				break;
			}
		}
	}

	//Logging
	if (true) {
		std::thread log(Log, nObj, nFunc, UE4::GetObjectFullName(static_cast<InternalUObject*>(pObj)->Class));
		log.detach();
	}

	return ProcessEvent(pObj, pFunc, pParams);
}

int Detours::GetViewPointDetour(void* pPlayer, FMinimalViewInfo* pViewInfo, BYTE stereoPass)
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
