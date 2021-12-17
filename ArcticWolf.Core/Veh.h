#pragma once

static class VEH
{
public:
    static bool Init();

    static bool AddHook(void* Target, void* Detour);

    static bool RemoveHook(void* Target);

    static bool IsSamePage(void* A, void* B);
};
