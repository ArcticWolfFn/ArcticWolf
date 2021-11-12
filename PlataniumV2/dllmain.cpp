#include "pch.h"

#include "util.h"
#include "hooks.h"


VOID WINAPI Main()
{
	Util::InitConsole();

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
			if (Hooks::Misc() && Console::Unlock())
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
