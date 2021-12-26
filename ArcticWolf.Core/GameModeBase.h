#pragma once
class AGameModeBase : public GIObject
{
public:
	AGameModeBase(InternalUObject* InternalObject);

	virtual void Setup() override;

public:
	InternalUObject* InternalObject = nullptr;
};

