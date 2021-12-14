#include "pch.h"
#include "Util.h"
#include <winscard.h>
#include <cstdio>
#include <Psapi.h>

BOOL Util::MaskCompare(PVOID pBuffer, LPCSTR lpPattern, LPCSTR lpMask)
{
	for (auto value = reinterpret_cast<PBYTE>(pBuffer); *lpMask; ++lpPattern, ++lpMask, ++value)
	{
		if (*lpMask == 'x' && *reinterpret_cast<LPCBYTE>(lpPattern) != *value)
			return false;
	}

	return true;
}

VOID Util::InitConsole()
{
	AllocConsole();

	FILE* pFile;
	freopen_s(&pFile, "CONOUT$", "w", stdout);
	freopen_s(&pFile, "CONIN$", "r", stdin);

	hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
}

uintptr_t Util::FindPattern(PVOID pBase, DWORD dwSize, LPCSTR lpPattern, LPCSTR lpMask)
{
	dwSize -= static_cast<DWORD>(strlen(lpMask));
	for (unsigned long index = 0; index < dwSize; ++index)
	{
		PBYTE pAddress = static_cast<PBYTE>(pBase) + index;
		if (MaskCompare(pAddress, lpPattern, lpMask)) return reinterpret_cast<uintptr_t>(pAddress);
	}
	return NULL;
}

bool Util::IsBadReadPtr(void* p)
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

uintptr_t Util::FindPattern(LPCSTR lpPattern, LPCSTR lpMask, BOOL SleepBetween)
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

std::wstring Util::sSplit(std::wstring s, std::wstring delimiter)
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
