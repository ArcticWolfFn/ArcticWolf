#pragma once

// Game Internal Object
class GIObject
{
public:
	// Setups the internal pointers to objects
	virtual void Setup();

protected:
	template <typename T> void SetPointer(wchar_t const* objectToFind, T* objectToSet, bool* success = nullptr);
};

