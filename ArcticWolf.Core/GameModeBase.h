#pragma once
class AGameModeBase : public GIObject
{
public:
	AGameModeBase(InternalUObject* InternalObject);

	virtual void Setup() override;

protected:
	InternalUObject* InternalObject = nullptr;
};

