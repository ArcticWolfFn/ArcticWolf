#pragma once

#include <cstdint>

// This struct is used on 4.20 and below. Not used on any future builds.
// We will call it TUObjectArray to prevent conflict.
struct TUObjectArray
{
	uint8_t* Objects;
	uint32_t MaxElements;
	uint32_t NumElements;
};