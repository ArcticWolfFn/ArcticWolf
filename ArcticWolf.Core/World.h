#pragma once

#include "EngineTypes.h"
#include "ObjectMacros.h"

enum ESpawnActorNameMode : uint8_t
{
	Required_Fatal,

	Required_ErrorAndReturnNull,

	Required_ReturnNull,

	Requested
};

struct FActorSpawnParameters
{
	FActorSpawnParameters();


	FName Name;

	UObject* Template; //AActor

	UObject* Owner; //AActor

	UObject* Instigator; //APawn

	UObject* OverrideLevel; //ULevel

	ESpawnActorCollisionHandlingMethod SpawnCollisionHandlingOverride;

private:

	uint8_t bRemoteOwned : 1;

public:

	bool IsRemoteOwned() const;

	uint8_t bNoFail : 1;


	uint8_t bDeferConstruction : 1;

	uint8_t bAllowDuringConstructionScript : 1;

	ESpawnActorNameMode NameMode;

	EObjectFlags ObjectFlags;
};

