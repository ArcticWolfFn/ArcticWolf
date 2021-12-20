#pragma once

struct EmptyParams {};

// Game Internal Object
class GIObject
{
public:
	GIObject();

	// Setups the internal pointers to objects
	virtual void Setup();

	template <typename T> void SetPointer(wchar_t const* objectToFind, T* objectToSet, bool* success = nullptr);

	static void ProcessNoParamsEvent(void* obj, void* fn);

private:
	static EmptyParams emptyParams;
};
