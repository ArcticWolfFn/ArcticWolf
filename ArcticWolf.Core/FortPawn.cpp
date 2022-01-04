#include "pch.h"
#include "FortPawn.h"

int32_t AFortPawn::Offset_CurrentMovementStyle = 0;

AFortPawn::AFortPawn(InternalUObject* InternalObject) : ACharacter(InternalObject)
{
}

void AFortPawn::Setup()
{
	__super::Setup();

	if (Offset_CurrentMovementStyle == 0)
	{
		Offset_CurrentMovementStyle = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortPawn"), XOR(L"CurrentMovementStyle"));
	}

	CurrentMovementStyle = reinterpret_cast<EFortMovementStyle*>(reinterpret_cast<uintptr_t>(this->InternalObject) + Offset_CurrentMovementStyle);
}

void AFortPawn::SetCurrentMovementStyle(EFortMovementStyle CurrentMovementStyle)
{
	if (Util::IsBadReadPtr(this->CurrentMovementStyle))
	{
		PLOGE << "CurrentMovementStyle is nullptr";
		return;
	}

	*this->CurrentMovementStyle = CurrentMovementStyle;
}
