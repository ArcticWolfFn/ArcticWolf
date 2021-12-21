#pragma once

struct EmptyParams {};

// Game Internal Object
class GIObject
{
public:
	GIObject();

	// Setups the internal pointers to objects
	virtual void Setup();

	static void ProcessNoParamsEvent(void* obj, void* fn);

private:
	static EmptyParams emptyParams;
};
