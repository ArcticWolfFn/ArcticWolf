#pragma once
#include "ue4.h"
#include <minwindef.h>
#include "neoroyale.h"
#include "ue4.h"
#include "player.h"
#include <thread>
#include "FortniteGame.h"

using namespace NeoRoyale;

namespace CameraHook
{
	inline float Speed = 0.1;
	inline float FOV = 52.0;
	inline FVector Camera(52.274170, 125912.695313, 89.249969);
	inline FRotator Rotation = { 0.870931, -88.071960, 0.008899 };
}

class Detours
{
public:
	void* ProcessEventDetour(UObject* pObj, UObject* pFunc, void* pParams);

private:
	bool bIsDebugCamera;
	bool bIsFlying;

	// Special async logging
	void Log(std::wstring nObj, std::wstring nFunc, std::wstring nObjClass);

	int GetViewPointDetour(void* pPlayer, FMinimalViewInfo* pViewInfo, BYTE stereoPass);
};


