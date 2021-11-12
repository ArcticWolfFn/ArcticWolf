#include "pch.h"

#include "util.h"
#include "hooks.h"


VOID WINAPI Main()
{
	Util::InitConsole();

	const char* logo =
		XOR(
			R"(
	
    ____  __      __              _               _    _____ 
   / __ \/ /___ _/ /_____ _____  (_)_  ______ ___| |  / /__ \
  / /_/ / / __ `/ __/ __ `/ __ \/ / / / / __ `__ \ | / /__/ /
 / ____/ / /_/ / /_/ /_/ / / / / / /_/ / / / / / / |/ // __/ 
/_/   /_/\__,_/\__/\__,_/_/ /_/_/\__,_/_/ /_/ /_/|___//____/ 

[=] Made by kemo(@xkem0x) with help from Mix (@ximton) and AsrielD (@Asriel_Dev)
[=] Use code Neonite in the item shop to support me #ad

)");

	printfc(FOREGROUND_BLUE, "%s", logo)
	printfc(FOREGROUND_GREEN, "[=] Built on: %s at %s\n", __DATE__, __TIME__);

	Hooks::Init();

	// disabled due to game not starting
	bool hit = false;
	while (true) {
		if (isReady) {
			if (hit == false) {
				printfc(FOREGROUND_GREEN, "Hit Hooks:Misc()!");
				hit = true;
			}
			if (Hooks::Misc())
			{
				break;
			}
		}
		Sleep(1000 / 30); // 30 fps
	}
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD dwReason, LPVOID lpReserved)
{
	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
		CreateThread(nullptr, 0, (LPTHREAD_START_ROUTINE)Main, hModule, 0, 0);
		//Main();
		break;
	case DLL_PROCESS_DETACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	default:
		break;
	}
	return TRUE;
}
