#pragma once
#include "ue4.h"
#include <minwindef.h>
#include "ue4.h"
#include "player.h"
#include <thread>

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
	static void* ProcessEventDetour(InternalUObject* pObj, InternalUObject* pFunc, void* pParams);

private:
	inline static bool bIsDebugCamera = false;
	inline static bool bIsFlying = false;
	inline static const wchar_t* ignoredFunctionNames[] = {
		L"EvaluateGraphExposedInputs",
		L"Tick",
		L"OnSubmixEnvelope",
		L"OnSubmixSpectralAnalysis",
		L"OnMouse",
		L"Pulse",
		L"BlueprintUpdateAnimation",
		L"BlueprintPostEvaluateAnimation",
		L"BlueprintModifyCamera",
		L"BlueprintModifyPostProcess",
		L"Loop Animation Curve",
		L"UpdateTime",
		L"GetMutatorByClass",
		L"UpdatePreviousPositionAndVelocity",
		L"IsCachedIsProjectileWeapon",
		L"LockOn",
		L"GetAbilityTargetingLevel",
		L"ServerTouchActiveTime",

		// UI
		L"OnAnimationFinished",
		L"OnAnimationStarted",
		L"SetColorAndOpacity",
		L"OnHover",
		L"OnHovered",
		L"OnUnhover",
		L"OnUnhovered",
		L"HandleButtonReleased",
		L"HandleButtonClicked",
		L"ScrollNextItem",
		L"OnCurrentTextStyleChanged",
		L"OnButtonUnhovered",
		L"OnButtonHovered",
		L"OnRemovedFromFocusPath",
		L"OnFocusLost",
		L"OnUpdateNameplateVis",
		L"/Script/UMG.Border.SetBrushColor",
		L"AthenaMOTDTeaserWidget.AthenaMOTDTeaserWidget_C.HandleEntryWidgetHoveredChanged",
		L"/Script/UMG.UserWidget.Destruct",

		// Camera
		L"OnFrontEndCameraChanged",
		L"FrontEndCameraSwitchFadeAthena__UpdateFunc",

		// Loading Stuff
		L"OnBuildingActorInitialized",
		L"OnReady",
		L"ReceiveBeginPlay",
		L"Construct",
		L"FortClientSettingsRecord",

		// InGame
		L"BlueprintGetInteractionTime",
		L"BGA_IslandPortal_C.CheckShouldDisplayUI",
		L"PortalInfoPlate_C.OnUpdateNameplateVis",
		L"FlopperSpawn",
		L"SetRuntimeStats",
		L"HandleSimulatingComponentHit",
		L"ReceiveHit",
		L"BGA_SuperSilkyWolf_C",
		L"ServerFireAIDirectorEvent",
		L"AnimNotify_FootStep",
		L"ReceiveDestroyed",
		L"AnimNotify",

		// Player
		L"/Script/Engine.Character.CanJumpInternal",

		// Interaction
		L"BlueprintCanInteract",
		L"BlueprintGetInteractionString",

		// InGame UI
		L"PopupCenterMessageWidget_C.UpdateStateEvent",
		L"FortInteractInterface.GetFocusedSocketLocation",

		// BattlePass UI

		// Called when a new item is sliding in
		L"BattlePassVaultWorld_C.Floor-Visibility__UpdateFunc",

		// I think this gets called if it shows a music pack, but music is muted
		L"B_MusicPackPreviewDisplay_C.UpdateMuteSetting",

		// Shutdown
		L"/Script/Engine.ActorComponent.ReceiveEndPlay",

		L"ReadyToEndMatch",
	};

	// Special async logging
	static void Log(std::wstring nObj, std::wstring nFunc, std::wstring nObjClass);

	static int GetViewPointDetour(void* pPlayer, FMinimalViewInfo* pViewInfo, BYTE stereoPass);
};


