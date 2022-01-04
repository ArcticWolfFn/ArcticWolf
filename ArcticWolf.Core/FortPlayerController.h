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

private:
	static UFunction* Fn_ServerReadyToStartMatch;

	static bool CanExec_ServerReadyToStartMatch;
};

