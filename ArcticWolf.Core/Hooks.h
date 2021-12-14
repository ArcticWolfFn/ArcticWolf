#ifndef HOOKS_H
#define HOOKS_H

#include "pch.h"

#include "url.h"
#include "curl.h"
#include "defs.h"
#include "veh.h"
#include "patterns.h"
#include "masks.h"
#include "ue4.h"
#include "util.h"
#include "patterns.h"
#include "patterns.h"
#include "detours.h"


//globals
static void* UnsafeEnvironmentPopupAddress;
static void* RequestExitWithStatusAddress;
static void* RequestExitAddress;
static uintptr_t CurlEasyAddress;
static uintptr_t CurlSetAddress;
static void* PushWidgetAddress;
//inline void* HotfixManagerInstance;
inline bool isReady = false;

//def
//static bool (*HotfixIniFile)(void* HotfixManager, const FString& FileName, const FString& IniData);

static CURLcode(*CurlSetOpt)(struct Curl_easy*, CURLoption, va_list);

static CURLcode(*CurlEasySetOpt)(struct Curl_easy*, CURLoption, ...);

static __int64 (*PushWidget)(__int64 WidgetInstance, const TCHAR* Body, const TCHAR* Widget, const TCHAR* WidgetType);

//hooks

inline CURLcode CurlSetOpt_(struct Curl_easy* data, CURLoption option, ...)
{
	va_list arg;
	va_start(arg, option);

	const auto result = CurlSetOpt(data, option, arg);

	va_end(arg);
	return result;
}

inline CURLcode CurlEasySetOptHook(struct Curl_easy* data, CURLoption tag, ...)
{
	va_list arg;
	va_start(arg, tag);

	CURLcode result = {};

	if (!data)
		return CURLE_BAD_FUNCTION_ARGUMENT;


	if (tag == CURLOPT_SSL_VERIFYPEER)
	{
		result = CurlSetOpt_(data, tag, 0);
	}

	// ToDo: only do this in production. Only in dev we would like to be able to see the stuff in Fiddler
	/*if (tag == CURLOPT_PROXY)
	{
		result = CurlSetOpt_(data, tag, "");
	}*/

	else if (tag == CURLOPT_URL)
	{
		std::string url = va_arg(arg, char*);
		//printfc(FOREGROUND_BLUE, "Url before: %s \n", url.c_str());

		Uri uri = Uri::Parse(url);

		if (url.find(XOR("ClientQuest")) != std::string::npos) isReady = true;

		if (uri.Host.ends_with("ol.epicgames.com")
			|| uri.Host.ends_with(".akamaized.net")
			|| uri.Host.ends_with(".epicgames.dev")
			|| uri.Host.ends_with("on.epicgames.com"))
		{
			//printf("LogURL: %s\n", url.c_str());
			if (strcmp(URL_HOST, XOR("localhost")) == 0)
			{
				std::string path = std::string(uri.Path).data();

				// akamai contains data like vod
				if (uri.Host.ends_with("akamaized.net")) {
					path = "/akamaized";
					path += std::string(uri.Path).data();
				}

				// aws contains data like the retryconfig after failed logins
				if (uri.Host.ends_with("amazonaws.com")) {
					path = "/aws";
					path += std::string(uri.Path).data();
				}

				url = Uri::CreateUri(URL_PROTOCOL, URL_HOST, URL_PORT, path.c_str(), uri.QueryString);
			}
			//printfc(FOREGROUND_BLUE, "Url after: %s \n", url.c_str());
		}


		result = CurlSetOpt_(data, tag, url.c_str());
	}

	else
	{
		result = CurlSetOpt(data, tag, arg);
	}

	va_end(arg);
	return result;
}

void RequestExitWithStatusHook(bool unknown, bool force)
{
	//printfc(FOREGROUND_BLUE, "[VEH] <REDACTED> Call IsForced: %i\n", force);
}

void RequestExitHook(bool force)
{
	printfc(FOREGROUND_BLUE, "[VEH] RequestExit Call IsForced: %i\n", force);
}

void UnsafeEnvironmentPopupHook(wchar_t** unknown1,
	unsigned __int8 _case,
	__int64 unknown2,
	char unknown3)
{
	//printfc(FOREGROUND_BLUE, "[VEH] <REDACTED> Call with Case: %i\n", _case);
}

/*__int64 PushWidgetHook(__int64 WidgetInstance, const TCHAR* Body, const TCHAR* Widget, const TCHAR* WidgetType)
{
	const std::wstring bodyW(Body);
	if (bodyW == L"Logging In...")
	{
		return PushWidget(WidgetInstance,
			XOR(L"\tPlataniumV2\n\tMade by kemo\n\tUse Code Neonite #ad"),
			Widget,
			WidgetType);
	}
	else if (bodyW == L"FILL")
	{
		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());
		DetourDetach(reinterpret_cast<void**>(&PushWidget), PushWidgetHook);
		DetourTransactionCommit();
	}
	return PushWidget(WidgetInstance, Body, Widget, WidgetType);
}*/

/*bool HotfixIniFileHook(void* HotfixManager, const FString& FileName, const FString& IniData)
{
	HotfixManagerInstance = HotfixManager;
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach(reinterpret_cast<void**>(&HotfixIniFile), HotfixIniFileHook);
	DetourTransactionCommit();
	return HotfixIniFile(HotfixManager, FileName, IniData);
}*/

namespace Hooks
{
	inline bool Init()
	{
		PLOGD << "Init started";

		CurlEasyAddress = Util::FindPattern(Patterns::CurlEasySetOpt.first, Patterns::CurlEasySetOpt.second);
		VALIDATE_ADDRESS(CurlEasyAddress, "Curl easy pattern is outdated.")

			CurlSetAddress = Util::FindPattern(Patterns::CurlSetOpt.first, Patterns::CurlSetOpt.second);
		VALIDATE_ADDRESS(CurlSetAddress, "Curl set pattern is outdated.")

			CurlEasySetOpt = decltype(CurlEasySetOpt)(CurlEasyAddress);

		CurlSetOpt = decltype(CurlSetOpt)(CurlSetAddress);


		if (VEH::Init())
		{
			VEH::AddHook(CurlEasySetOpt, CurlEasySetOptHook);
		}
	}

	inline bool Misc() {
		PLOGV << "Start Hooks::Misc";

		if (MH_Initialize() != MH_OK)
		{
			MessageBoxA(nullptr, XOR("Failed to initialize min-hook, terminating the thread."), XOR("Cranium"), MB_OK);
			FreeLibraryAndExitThread(GetModuleHandle(nullptr), 0);
		}

		//Used to find objects, dump them, mostly works as an alternative for the ObjectFinder.
		auto GObjectsAdd = Util::FindPattern(Patterns::bGlobal::GObjects, Masks::bGlobal::GObjects);
		VALIDATE_ADDRESS(GObjectsAdd, XOR("Failed to find GObjects Address."));

		GObjs = decltype(GObjs)(RELATIVE_ADDRESS(GObjectsAdd, 7));

		// Engine
		auto GEngineAdd = Util::FindPattern(Patterns::bGlobal::GEngine, Masks::bGlobal::GEngine);
		VALIDATE_ADDRESS(GEngineAdd, XOR("Failed to find GEngine Address."));

		GEngine = *reinterpret_cast<UEngine**>(GEngineAdd + 7 + *reinterpret_cast<int32_t*>(GEngineAdd + 3));

		//Used for ProcessEvent Hooking.
		auto ProcessEventAdd = Util::FindPattern(Patterns::bGlobal::ProcessEvent, Masks::bGlobal::ProcessEvent);
		VALIDATE_ADDRESS(ProcessEventAdd, XOR("Failed to find ProcessEvent Address."));

		ProcessEvent = decltype(ProcessEvent)(ProcessEventAdd);

		gProcessEventAdd = ProcessEventAdd;

		//Used for Camera Hooking.
		auto GetViewPointAdd = Util::FindPattern(Patterns::bGlobal::GetViewPoint, Masks::bGlobal::GetViewPoint);
		VALIDATE_ADDRESS(GetViewPointAdd, XOR("Failed to find GetViewPoint Address."));

		GetViewPoint = decltype(GetViewPoint)(GetViewPointAdd);

		//Used for getting UObjects names.
		auto GONIAdd = Util::FindPattern(Patterns::bGlobal::GONI, Masks::bGlobal::GONI);
		VALIDATE_ADDRESS(GONIAdd, XOR("Failed to find GetObjectName Address."));

		GetObjectNameInternal = decltype(GetObjectNameInternal)(GONIAdd);

		//Used for getting UObjects full names.
		auto GetObjectFullNameAdd = Util::FindPattern(Patterns::bGlobal::GetObjectFullName, Masks::bGlobal::GetObjectFullName);
		VALIDATE_ADDRESS(GetObjectFullNameAdd, XOR("Failed to find GetObjectFullName Address."));

		GetObjectFullNameInternal = decltype(GetObjectFullNameInternal)(GetObjectFullNameAdd);

		//Used for getting FFields full names.
		auto GetFullNameAdd = Util::FindPattern(Patterns::bGlobal::GetFullName, Masks::bGlobal::GetFullName);
		VALIDATE_ADDRESS(GetFullNameAdd, XOR("Failed to find GetFullName Address."));

		GetFullName = decltype(GetFullName)(GetFullNameAdd);


		//Used to free the memory for names.
		auto FreeInternalAdd = Util::FindPattern(Patterns::bGlobal::FreeInternal, Masks::bGlobal::FreeInternal);
		VALIDATE_ADDRESS(FreeInternalAdd, XOR("Failed to find Free Address."));

		FreeInternal = decltype(FreeInternal)(FreeInternalAdd);


		//Used to construct objects, mostly used for console stuff.
		auto SCOIAdd = Util::FindPattern(Patterns::bGlobal::SCOI, Masks::bGlobal::SCOI);
		VALIDATE_ADDRESS(SCOIAdd, XOR("Failed to find SCOI Address."));

		StaticConstructObject = decltype(StaticConstructObject)(SCOIAdd);

		//Used to load objects.
		auto SLOIAdd = Util::FindPattern(Patterns::bGlobal::SLOI, Masks::bGlobal::SLOI);
		VALIDATE_ADDRESS(SLOIAdd, XOR("Failed to find SLOI Address."));

		StaticLoadObject = decltype(StaticLoadObject)(SLOIAdd);


		auto AbilityPatchAdd = Util::FindPattern(Patterns::bGlobal::AbilityPatch, Masks::bGlobal::AbilityPatch);
		VALIDATE_ADDRESS(AbilityPatchAdd, XOR("Failed to find AbilityPatch Address."));

		//Patches fortnite ability ownership checks, work on everysingle fortnite version.
		//Author: @nyamimi
		reinterpret_cast<uint8_t*>(AbilityPatchAdd)[2] = 0x85;
		reinterpret_cast<uint8_t*>(AbilityPatchAdd)[11] = 0x8D;

		//Process Event Hooking.
		MH_CreateHook(reinterpret_cast<void*>(ProcessEventAdd), ProcessEventDetour, reinterpret_cast<void**>(&ProcessEvent));
		MH_EnableHook(reinterpret_cast<void*>(ProcessEventAdd));

		auto Map = APOLLO_TERRAIN;

		gPlaylist = UE4::FindObject<UObject*>(XOR(L"FortPlaylistAthena /Game/Athena/Playlists/BattleLab/Playlist_BattleLab.Playlist_BattleLab"));
		if (gPlaylist == nullptr) {
			PLOGE << "Hooks: Playlist is null";
		}
		else {
			auto nObj = UE4::GetObjectFullName(gPlaylist);
			PLOGV.printf("Hooks: Playlist points to %s", nObj.c_str());
		}

		PLOGV << "End Hooks::Misc";

		return true;
	}
}

#endif // HOOKS_H


