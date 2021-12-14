#pragma once

#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <Psapi.h>
#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <d3d11.h>
#include <dxgi.h>
#include <vector>
#include <stdio.h>
#include <string.h>
#include <winsock2.h>
#include <string>
#include <algorithm>
#include <vector>
#include <fcntl.h>
#include <time.h>
#include <stdint.h>

#include "xorstr.hpp"

#define XOR(STR) xorstr(STR).crypt_get()
#define _(str) xorstr_(str)

#pragma comment(lib, "minhook.lib")
#pragma comment(lib, "user32.lib")
#pragma comment(lib, "winhttp.lib")
#pragma comment(lib, "advapi32.lib")
#pragma comment(lib, "ws2_32.lib") 
#include "MinHook.h"

#include <plog/Log.h>