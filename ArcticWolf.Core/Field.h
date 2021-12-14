#pragma once

#include "NameTypes.h"
#include "ObjectMacros.h"

class FField
{
	void* vtable;
	void* Class;
	void* Owner;
	void* padding;
	FField* Next;
	FName NamePrivate;
	EObjectFlags FlagsPrivate;
};

