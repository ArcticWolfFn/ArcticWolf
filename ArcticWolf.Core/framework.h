#pragma once

#define WIN32_LEAN_AND_MEAN             // Selten verwendete Komponenten aus Windows-Headern ausschließen
// Windows-Headerdateien
#include <windows.h>
#include <cstdint>
#include <stdint.h>
#include "Object.h"
#include "Class.h"
#include "xorstr.hpp"
#include "UE4.h"
#include "FortniteGameInstance.h"

#include <plog/Log.h>

#pragma comment(lib, "user32.lib")
#pragma comment(lib, "winhttp.lib")
#pragma comment(lib, "advapi32.lib")
#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "minhook.lib")

inline FortniteGameInstance GGameInstance = FortniteGameInstance();