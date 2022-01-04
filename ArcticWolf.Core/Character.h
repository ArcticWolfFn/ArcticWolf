#pragma once
class ACharacter : GIObject
{
public:
	ACharacter();
	ACharacter(InternalUObject* InternalObject);
	virtual void Setup() override;

protected:
	InternalUObject* InternalObject = nullptr;
};

