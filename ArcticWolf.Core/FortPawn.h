#pragma once

#include "Character.h"

enum class EFortMovementStyle : uint8_t {
	Running,
	Walking,
	Charging,
	Sprinting,
	PersonalVehicle,
	Flying,
	Tethered,
	Burrowing,
	EFortMovementStyle_MAX,
};

class AFortPawn : ACharacter
{
public:
	AFortPawn(InternalUObject* InternalObject);

	virtual void Setup() override;

	void SetCurrentMovementStyle(EFortMovementStyle CurrentMovementStyle);

private:
	EFortMovementStyle* CurrentMovementStyle = nullptr;

	static int32_t Offset_CurrentMovementStyle;
};

