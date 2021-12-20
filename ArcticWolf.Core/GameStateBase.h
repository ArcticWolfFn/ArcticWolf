#pragma once
class AGameStateBase : protected GIObject
{
public:
	AGameStateBase(UObject* InternalObject);

	virtual void Setup() override;

protected:
	UObject* InternalObject;
};

