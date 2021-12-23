#pragma once

#include "FortKismetLibrary.h"
#include "GameInstance.h"

using namespace FortniteGame;

class FortniteGameInstance : public UGameInstance
{
public:
	UFortKismetLibrary FortKismetLibrary;

	void Setup() override;
};

