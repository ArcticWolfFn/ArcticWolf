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
	static UFunction* Fn_K2_DestroyActor;

	static bool CanExec_K2_DestroyActor;
};

