#include "pch.h"
#include "Rotator.h"

FRotator::FRotator() : Pitch(0), Yaw(0), Roll(0)
{
}

FRotator::FRotator(float pitch, float yaw, float roll) : Pitch(pitch), Yaw(yaw), Roll(roll)
{
}