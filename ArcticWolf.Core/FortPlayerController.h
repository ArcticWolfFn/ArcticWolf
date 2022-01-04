#pragma once

#include "PlayerController.h"

class AFortPlayerController : public APlayerController
{
public:
	AFortPlayerController();
	AFortPlayerController(APlayerController* PlayerController);

	void Setup() override;

	// (Net|NetReliableNative|Event|Protected|NetServer|NetValidate)
	void ServerReadyToStartMatch();

	void ServerSetClientHasFinishedLoading(bool bInHasFinishedLoading);

	void OnRep_bHasServerFinishedLoading();

	void SetbHasServerFinishedLoading(bool bHasServerFinishedLoading);

private:
	static UFunction* Fn_ServerReadyToStartMatch;
	static UFunction* Fn_ServerSetClientHasFinishedLoading;
	static UFunction* Fn_OnRep_bHasServerFinishedLoading;

	static bool CanExec_ServerReadyToStartMatch;
	static bool CanExec_ServerSetClientHasFinishedLoading;
	static bool CanExec_OnRep_bHasServerFinishedLoading;

	static int32_t Offset_bHasServerFinishedLoading;

	bool* bHasServerFinishedLoading = nullptr;
};

