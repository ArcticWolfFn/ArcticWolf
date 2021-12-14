#pragma once

enum ESpawnActorCollisionHandlingMethod : uint8_t
{
    Undefined,
    AlwaysSpawn,
    AdjustIfPossibleButAlwaysSpawn,
    AdjustIfPossibleButDontSpawnIfColliding,
    DontSpawnIfColliding,
    ESpawnActorCollisionHandlingMethod_MAX,
};

