#pragma once

class APawn;

class AController : protected GIObject
{
public:
	AController();
	AController(ObjectFinder* ControllerFinder);
	AController(InternalUObject*& InternalObject);

	virtual void Setup() override;

	void Possess(APawn* InPawn);

	InternalUObject*& InternalObject;

private:
	static UFunction* Fn_Possess;

	static bool CanExec_Possess;
};

