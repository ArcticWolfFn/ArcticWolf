#pragma once

struct EmptyParams {};

class InternalUObject;

// Game Internal Object
class GIObject
{
public:
	GIObject();

	// Setups the internal pointers to objects
	virtual void Setup();

	static void ProcessNoParamsEvent(void* obj, void* fn);

	// hack for compile errors or something like that
	InternalUObject*& toPointerReference(InternalUObject* pointer);

private:
	static EmptyParams emptyParams;
};
