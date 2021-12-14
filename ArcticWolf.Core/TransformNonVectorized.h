#pragma once

#include "Quat.h"
#include "Vector.h"

struct FTransform
{
	FQuat Rotation;
	FVector Translation;
	char UnknownData_1C[0x4];
	FVector Scale3D;
	char UnknownData_2C[0x4];
};

