#pragma once
class AGameModeBase : public GIObject
{
public:
	AGameModeBase(UObject* InternalObject);

	virtual void Setup() override;

protected:
	UObject* InternalObject = nullptr;
};

