#pragma once

#include "pch.h"
#include <winscard.h>
#include <cstdio>

inline HANDLE hConsole;

#define SET_COLOR(c) SetConsoleTextAttribute(hConsole, c);

#define printfc(c, ...) SET_COLOR(c) printf(__VA_ARGS__);

#define FIRST_TIME_HERE ([] { \
    static bool is_first_time = true; \
    auto was_first_time = is_first_time; \
    is_first_time = false; \
    return was_first_time; } ())

#define VALIDATE_ADDRESS(address, error) \
    if (!address) { \
		printfc(FOREGROUND_RED, error) \
        MessageBoxA(0, error, XOR("PlataniumV2"), MB_OK); \
		FreeLibraryAndExitThread(GetModuleHandle(NULL), 0); \
        return 0; \
    }

#define RELATIVE_ADDRESS(address, size) ((PBYTE)((UINT_PTR)(address) + *(PINT)((UINT_PTR)(address) + ((size) - sizeof(INT))) + (size)))

#define DetoursEasy(address, hook) \
	DetourTransactionBegin(); \
    DetourUpdateThread(GetCurrentThread()); \
    DetourAttach(reinterpret_cast<void**>(&address), hook); \
    DetourTransactionCommit();

#define READ_DWORD(base, offset) (*(PDWORD)(((PBYTE)base + offset)))

#define READ_POINTER(base, offset) (*(PVOID *)(((PBYTE)base + offset)))


class Util
{
private:
	static inline BOOL MaskCompare(PVOID pBuffer, LPCSTR lpPattern, LPCSTR lpMask)
	{
		for (auto value = reinterpret_cast<PBYTE>(pBuffer); *lpMask; ++lpPattern, ++lpMask, ++value)
		{
			if (*lpMask == 'x' && *reinterpret_cast<LPCBYTE>(lpPattern) != *value)
				return false;
		}

		return true;
	}

public:
	static inline VOID InitConsole()
	{
		AllocConsole();

		freopen("output.txt", "w", stdout);
		freopen("error.txt", "w", stderr);
		//freopen_s(File, "CONIN$", "r", stdin);
		hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	}

	static __forceinline uintptr_t FindPattern(PVOID pBase, DWORD dwSize, LPCSTR lpPattern, LPCSTR lpMask)
	{
		dwSize -= static_cast<DWORD>(strlen(lpMask));
		for (unsigned long index = 0; index < dwSize; ++index)
		{
			PBYTE pAddress = static_cast<PBYTE>(pBase) + index;
			if (MaskCompare(pAddress, lpPattern, lpMask)) return reinterpret_cast<uintptr_t>(pAddress);
		}
		return NULL;
	}

	static __forceinline bool IsBadReadPtr(void* p)
	{
		MEMORY_BASIC_INFORMATION mbi;
		if (VirtualQuery(p, &mbi, sizeof(mbi)))
		{
			DWORD mask = (PAGE_READONLY | PAGE_READWRITE | PAGE_WRITECOPY | PAGE_EXECUTE_READ | PAGE_EXECUTE_READWRITE | PAGE_EXECUTE_WRITECOPY);
			bool b = !(mbi.Protect & mask);
			if (mbi.Protect & (PAGE_GUARD | PAGE_NOACCESS)) b = true;

			return b;
		}
		return true;
	}

	static __forceinline uintptr_t FindPattern(LPCSTR lpPattern, LPCSTR lpMask, BOOL SleepBetween = false)
	{
		MODULEINFO info = { nullptr };
		GetModuleInformation(GetCurrentProcess(), GetModuleHandle(nullptr), &info, sizeof(info));

		uintptr_t pAddr = 0;

		do
		{
			pAddr = FindPattern(info.lpBaseOfDll, info.SizeOfImage, lpPattern, lpMask);

			Sleep(50);
		} while (!pAddr);

		return pAddr;
	}

	static __forceinline std::wstring sSplit(std::wstring s, std::wstring delimiter)
	{
		size_t pos;
		std::wstring token;
		while ((pos = s.find(delimiter)) != std::string::npos)
		{
			token = s.substr(0, pos);
			return token;
		}
		return token;
	}
	
	static std::string base64_decode(const std::string& in) {

		std::string out;

		std::vector<int> T(256, -1);
		for (int i = 0; i < 64; i++) T["ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[i]] = i;

		int val = 0, valb = -8;
		for (unsigned char c : in) {
			if (T[c] == -1) break;
			val = (val << 6) + T[c];
			valb += 6;
			if (valb >= 0) {
				out.push_back(char((val >> valb) & 0xFF));
				valb -= 8;
			}
		}
		return out;
	}
};
