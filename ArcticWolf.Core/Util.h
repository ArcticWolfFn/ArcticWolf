#pragma once

#include <plog/Log.h>

class UE4;

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
	static BOOL MaskCompare(PVOID pBuffer, LPCSTR lpPattern, LPCSTR lpMask);

public:
	static VOID InitConsole();

	static uintptr_t FindPattern(PVOID pBase, DWORD dwSize, LPCSTR lpPattern, LPCSTR lpMask);

	static bool IsBadReadPtr(void* p);

	static uintptr_t FindPattern(LPCSTR lpPattern, LPCSTR lpMask, BOOL SleepBetween = false);

	static std::wstring sSplit(std::wstring s, std::wstring delimiter);
};

template <class T>
inline void SetPointer(const wchar_t* objectToFind, T* objectToSet, bool* success = nullptr)
{
	T obj = UE4::FindObject<T>(objectToFind);

	if (Util::IsBadReadPtr(obj))
	{
		PLOGE.printf("%s is nullptr", objectToFind);
		if (success != nullptr)
		{
			*success = false;
		}
		return;
	}

	objectToSet = &obj;
	if (success != nullptr)
	{
		*success = true;
	}
}
