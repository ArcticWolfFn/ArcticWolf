#pragma once
class AActor : public UObject
{
public:
	AActor();
	AActor(UObject object);

	virtual void Setup() override;

	// (Native|Public|BlueprintCallable)
	virtual void K2_DestroyActor();

protected:
	UObject* InternalObject = nullptr;

private:
	UFunction* Fn_K2_DestroyActor = nullptr;

	bool CanExec_K2_DestroyActor = false;
};

