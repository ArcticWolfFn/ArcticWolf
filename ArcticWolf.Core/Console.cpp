#include "pch.h"
#include "Console.h"
#include "Finder.h"
#include "CheatManager.h"

bool Console::CheatManager()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

	if (!LocalPlayer.GetObj()) return false;

	ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));

	ObjectFinder CheatManagerFinder = PlayerControllerFinder.Find(XOR(L"CheatManager"));

	InternalUObject*& pcCheatManager = reinterpret_cast<InternalUObject*&>(CheatManagerFinder.GetObj());

	auto cCheatManager = UE4::FindObject<InternalUClass*>(XOR(L"Class /Script/Engine.CheatManager"));

	if (!pcCheatManager && cCheatManager)
	{
		auto CheatManager = StaticConstructObject(
			cCheatManager,
			PlayerControllerFinder.GetObj(),
			nullptr,
			RF_NoFlags,
			EInternalObjectFlags::None,
			nullptr,
			false,
			nullptr,
			false
		);

		pcCheatManager = CheatManager;

		// Set CheatManager to the new object
		GetGame()->LocalPlayers[0].PlayerController.CheatManager = UCheatManager(CheatManager);
		GetGame()->LocalPlayers[0].PlayerController.CheatManager.Setup();

		PLOGI << "Player now has cheatmanager";

		return true;
	}
	return false;
}

bool Console::Unlock()
{
	ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
	ObjectFinder ConsoleClassFinder = EngineFinder.Find(XOR(L"ConsoleClass"));
	ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
	ObjectFinder ViewportConsoleFinder = GameViewPortClientFinder.Find(XOR(L"ViewportConsole"));

	InternalUObject*& ViewportConsole = reinterpret_cast<InternalUObject*&>(ViewportConsoleFinder.GetObj());

	auto Console = StaticConstructObject(
		static_cast<InternalUClass*>(ConsoleClassFinder.GetObj()),
		reinterpret_cast<InternalUObject*>(GameViewPortClientFinder.GetObj()),
		nullptr,
		RF_NoFlags,
		EInternalObjectFlags::None,
		nullptr,
		false,
		nullptr,
		false
	);

	ViewportConsole = Console;

	CheatManager();
	return true;
}