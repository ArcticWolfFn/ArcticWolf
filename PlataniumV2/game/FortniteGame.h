#pragma once

#include "GIObject.h"
#include "FortniteGame.h"
#include "Fort/FortPlayerCharacter.h"

using namespace Game;

static class FortniteGame
{
public:
	static Fort::FortPlayerCharacter PlayerCharacter;

	static void Setup();
};