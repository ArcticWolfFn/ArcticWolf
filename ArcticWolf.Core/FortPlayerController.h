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
	UFunction* Fn_ServerReadyToStartMatch = nullptr;

	bool CanExec_ServerReadyToStartMatch = false;
};

