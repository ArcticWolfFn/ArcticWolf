#pragma once
#include "ue4.h"
#include <minwindef.h>
#include "neoroyale.h"
#include "ue4.h"
#include "player.h"
#include <thread>

using namespace NeoRoyale;

namespace CameraHook
{
	inline float Speed = 0.1;
	inline float FOV = 52.0;
	inline FVector Camera(52.274170, 125912.695313, 89.249969);
	inline FRotator Rotation = { 0.870931, -88.071960, 0.008899 };
}

static class Detours
{
public:
	static void* ProcessEventDetour(UObject* pObj, UObject* pFunc, void* pParams);

private:
	static bool bIsDebugCamera;
	static bool bIsFlying;

	// Special async logging
	static void Log(std::wstring nObj, std::wstring nFunc, std::wstring nObjClass);

	static int GetViewPointDetour(void* pPlayer, FMinimalViewInfo* pViewInfo, BYTE stereoPass);
};


