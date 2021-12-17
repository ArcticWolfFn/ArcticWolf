#pragma once

class VEH
{
public:
    bool Init();

    bool AddHook(void* Target, void* Detour);

    bool RemoveHook(void* Target);

    bool IsSamePage(void* A, void* B)
};
