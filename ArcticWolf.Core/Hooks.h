#include "pch.h"

#include "Url.h"
#include "curl.h"
#include "Veh.h"
#include "Patterns.h"
#include "Masks.h"
#include "ue4.h"
#include "util.h"
#include "Detours.h"
#include "MinHook.h"
#include "Match.h"
#include "FortPlaylistAthena.h"

inline const char* URL_PROTOCOL = "https";
inline const char* URL_HOST = "localhost";
inline const char* URL_PORT = "44366";

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

static class Hooks
{
public:
	static bool Init()
	{
		PLOGD << "Init started";

		CurlEasyAddress = Util::FindPattern(Patterns::Curl::CurlEasySetOpt.first, Patterns::Curl::CurlEasySetOpt.second);
		VALIDATE_ADDRESS(CurlEasyAddress, "Curl easy pattern is outdated.")

		CurlSetAddress = Util::FindPattern(Patterns::Curl::CurlSetOpt.first, Patterns::Curl::CurlSetOpt.second);
		VALIDATE_ADDRESS(CurlSetAddress, "Curl set pattern is outdated.")

			CurlEasySetOpt = decltype(CurlEasySetOpt)(CurlEasyAddress);

		CurlSetOpt = decltype(CurlSetOpt)(CurlSetAddress);


		if (VEH::Init())
		{
			VEH::AddHook(CurlEasySetOpt, CurlEasySetOptHook);
		}

		PLOGD << "Init finished";
	}

	static bool Misc() {
		PLOGV << "Start Hooks::Misc";

		if (MH_Initialize() != MH_OK)
		{
			MessageBoxA(nullptr, XOR("Failed to initialize min-hook, terminating the thread."), XOR("Cranium"), MB_OK);
			FreeLibraryAndExitThread(GetModuleHandle(nullptr), 0);
		}

		//Used to find objects, dump them, mostly works as an alternative for the ObjectFinder.
		auto GObjectsAdd = Util::FindPattern(Patterns::Global::GObjects, Masks::Global::GObjects);
		VALIDATE_ADDRESS(GObjectsAdd, XOR("Failed to find GObjects Address."));

		GObjs = decltype(GObjs)(RELATIVE_ADDRESS(GObjectsAdd, 7));

		// Engine
		auto GEngineAdd = Util::FindPattern(Patterns::Global::GEngine, Masks::Global::GEngine);
		VALIDATE_ADDRESS(GEngineAdd, XOR("Failed to find GEngine Address."));

		GEngine = *reinterpret_cast<UEngine**>(GEngineAdd + 7 + *reinterpret_cast<int32_t*>(GEngineAdd + 3));

		//Used for Camera Hooking.
		auto GetViewPointAdd = Util::FindPattern(Patterns::Global::GetViewPoint, Masks::Global::GetViewPoint);
		VALIDATE_ADDRESS(GetViewPointAdd, XOR("Failed to find GetViewPoint Address."));

		GetViewPoint = decltype(GetViewPoint)(GetViewPointAdd);

		//Used for getting UObjects names.
		auto GONIAdd = Util::FindPattern(Patterns::Global::GONI, Masks::Global::GONI);
		VALIDATE_ADDRESS(GONIAdd, XOR("Failed to find GetObjectName Address."));

		GetObjectNameInternal = decltype(GetObjectNameInternal)(GONIAdd);

		//Used for getting UObjects full names.
		auto GetObjectFullNameAdd = Util::FindPattern(Patterns::Global::GetObjectFullName, Masks::Global::GetObjectFullName);
		VALIDATE_ADDRESS(GetObjectFullNameAdd, XOR("Failed to find GetObjectFullName Address."));

		GetObjectFullNameInternal = decltype(GetObjectFullNameInternal)(GetObjectFullNameAdd);

		//Used for getting FFields full names.
		auto GetFullNameAdd = Util::FindPattern(Patterns::Global::GetFullName, Masks::Global::GetFullName);
		VALIDATE_ADDRESS(GetFullNameAdd, XOR("Failed to find GetFullName Address."));

		GetFullName = decltype(GetFullName)(GetFullNameAdd);


		//Used to free the memory for names.
		auto FreeInternalAdd = Util::FindPattern(Patterns::Global::FreeInternal, Masks::Global::FreeInternal);
		VALIDATE_ADDRESS(FreeInternalAdd, XOR("Failed to find Free Address."));

		FreeInternal = decltype(FreeInternal)(FreeInternalAdd);


		//Used to construct objects, mostly used for console stuff.
		auto SCOIAdd = Util::FindPattern(Patterns::Global::SCOI, Masks::Global::SCOI);
		VALIDATE_ADDRESS(SCOIAdd, XOR("Failed to find SCOI Address."));

		StaticConstructObject = decltype(StaticConstructObject)(SCOIAdd);

		//Used to load objects.
		auto SLOIAdd = Util::FindPattern(Patterns::Global::SLOI, Masks::Global::SLOI);
		VALIDATE_ADDRESS(SLOIAdd, XOR("Failed to find SLOI Address."));

		StaticLoadObject = decltype(StaticLoadObject)(SLOIAdd);

		/*PLOGI << "Trying to find pattern";
		auto SpawnActorAdd = Util::FindPattern(Patterns::Global::SpawnActorInternal,
			Masks::Global::SpawnActorInternal);
		PLOGI << "Maybe found a pattern";
		VALIDATE_ADDRESS(SpawnActorAdd, XOR("Failed to find SpawnActor Address."));
		PLOGI << "Validated address";

		SpawnActor = decltype(SpawnActor)(SpawnActorAdd);*/


		auto AbilityPatchAdd = Util::FindPattern(Patterns::Global::AbilityPatch, Masks::Global::AbilityPatch);
		VALIDATE_ADDRESS(AbilityPatchAdd, XOR("Failed to find AbilityPatch Address."));

		//Used for ProcessEvent Hooking.
		auto ProcessEventAdd = Util::FindPattern(Patterns::Global::ProcessEvent, Masks::Global::ProcessEvent);
		VALIDATE_ADDRESS(ProcessEventAdd, XOR("Failed to find ProcessEvent Address."));

		ProcessEvent = decltype(ProcessEvent)(ProcessEventAdd);

		gProcessEventAdd = ProcessEventAdd;

		//Patches fortnite ability ownership checks, work on everysingle fortnite version.
		//Author: @nyamimi
		reinterpret_cast<uint8_t*>(AbilityPatchAdd)[2] = 0x85;
		reinterpret_cast<uint8_t*>(AbilityPatchAdd)[11] = 0x8D;

		//Process Event Hooking.
		MH_CreateHook(reinterpret_cast<void*>(ProcessEventAdd), Detours::ProcessEventDetour, reinterpret_cast<void**>(&ProcessEvent));
		MH_EnableHook(reinterpret_cast<void*>(ProcessEventAdd));

		auto Map = XOR(L"Apollo_Terrain?game=/Game/Athena/Athena_GameMode.Athena_GameMode_C");

		/*gPlaylist = UE4::FindObject<UFortPlaylistAthena*>(XOR(L"FortPlaylistAthena /Game/Athena/Playlists/BattleLab/Playlist_BattleLab.Playlist_BattleLab"));
		if (gPlaylist == nullptr) {
			PLOGE << "Hooks: Playlist is null";
		}
		else {
			auto nObj = UE4::GetObjectFullName(gPlaylist);
			PLOGV.printf("Hooks: Playlist points to %s", nObj.c_str());
		}*/

		GGameEngine.Setup();

		PLOGV << "End Hooks::Misc";

		return true;
	}
};


