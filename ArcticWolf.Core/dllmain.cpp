#include "pch.h"
#include "Util.h"
#include <plog/Appenders/ColorConsoleAppender.h>
#include <plog/Formatters/TxtFormatter.h>
#include "plog/Initializers/RollingFileInitializer.h"
#include "Hooks.h"
#include "Console.h"

VOID WINAPI Main()
{
	Util::InitConsole();

	static plog::ColorConsoleAppender<plog::TxtFormatter> consoleAppender;
	static plog::RollingFileAppender<plog::TxtFormatter> fileAppender("arctic.log");

	plog::init(plog::verbose, &consoleAppender).addAppender(&fileAppender);

	PLOGI.printf("Built on: %s at %s", __DATE__, __TIME__);

	Hooks::Init();

	// disabled due to game not starting
	bool hit = false;
	while (true) {
		if (isReady) {
			if (hit == false) {
				LOG_INFO << "Hit Hooks:Misc()!";
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

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
		CreateThread(nullptr, 0, (LPTHREAD_START_ROUTINE)Main, hModule, 0, 0);
		break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
	default:
		break;
    }
    return TRUE;
}

