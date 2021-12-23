#pragma once
#include "pch.h"
#include "ue4.h"
#include "util.h"
#include "ue4.h"
#include "UObjectArray.h"
#include "GObjects.h"

namespace Offsets
{
	inline static constexpr int32_t OffsetInternal = 0x4C;
	inline static constexpr int32_t Next = 0x20;
	inline static constexpr int32_t ClassPrivate = 0x10;
	inline static constexpr int32_t ChildProperties = 0x50;
	inline static constexpr int32_t SuperStruct = 0x40;
}

class GameObject;

inline TUObjectArray* ObjObjects = nullptr;

inline GObjects* GlobalObjects = nullptr;

class GameClass
{
public:
	GameObject* GetChildProperties()
	{
		return *reinterpret_cast<GameObject**>(this + Offsets::ChildProperties);
	}

	GameClass* GetSuperStruct()
	{
		return *reinterpret_cast<GameClass**>(this + Offsets::SuperStruct);
	}
};

class GameObject
{
public:
	GameClass* GetClass()
	{
		return *reinterpret_cast<GameClass**>(this + Offsets::ClassPrivate);
	}

	GameObject* GetNext()
	{
		return *reinterpret_cast<GameObject**>(this + Offsets::Next);
	}

	int32_t GetOffsetInternal()
	{
		return *reinterpret_cast<int32_t*>(this + Offsets::OffsetInternal);
	}
};

class ObjectFinder
{
	std::wstring m_currentObject;
	std::wstring m_objectType;
	GameObject* m_object;
	GameObject*& m_objectRef;

	static GameObject* InternalFindChildInObject(GameObject* inObject, std::wstring_view childName);

	GameObject*& resolveValuePointer(GameObject* bastePtr, GameObject* prop) const;

	GameObject*& resolveArrayValuePointer(GameObject* bastePtr, GameObject* prop) const;

public:
	ObjectFinder(const std::wstring& currentObject, const std::wstring objectType, GameObject* object, GameObject*& objectRef);

	InternalUObject*& GetObj() const;

	static ObjectFinder EntryPoint(uintptr_t EntryPointAddress);

	ObjectFinder Find(const std::wstring& objectToFind) const;

	static int32_t FindOffset(const std::wstring& classToFind, const std::wstring& objectToFind);

	ObjectFinder FindChildObject(const std::wstring& objectToFind) const;

	static InternalUObject* FindActor(std::wstring name, int toSkip = 0);

	static void DestroyActor(std::wstring name);
};
