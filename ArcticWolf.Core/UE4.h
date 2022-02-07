/**
 * Copyright (c) 2020-2021 Kareem Olim (Kemo)
 * All Rights Reserved. Licensed under the Neo License
 * https://neonite.dev/LICENSE.html
 */

#pragma once
#include "CameraTypes.h"
#include "Object.h"
#include "TransformNonVectorized.h"
#include "World.h"
#include "UObjectBaseUtility.h"
#include "GObjects.h"
#include <fstream>
#include <iostream>
#include "Field.h"
#include <algorithm>

class Util;

inline void* (*ProcessEvent)(void*, void*, void*);
inline int (*GetViewPoint)(void*, FMinimalViewInfo*, BYTE);
inline FString(*GetObjectNameInternal)(const InternalUObject* Object);
inline InternalUObject* (*SpawnActor)(InternalUObject* UWorld, InternalUClass* Class, FTransform const* UserTransformPtr, const FActorSpawnParameters& SpawnParameters);
inline void (*GetFullName)(FField* Obj, FString& ResultString, const UObject* StopOuter, EObjectFullNameFlags Flags);
inline void (*GetObjectFullNameInternal)(InternalUObject* Obj, FString& ResultString, const InternalUObject* StopOuter, EObjectFullNameFlags Flags);
inline void (*FreeInternal)(void*);
inline GObjects* GObjs;
inline struct UEngine* GEngine;

inline InternalUObject* (*StaticConstructObject)(
	InternalUClass* Class,
	InternalUObject* InOuter,
	void* Name,
	EObjectFlags SetFlags,
	EInternalObjectFlags InternalSetFlags,
	UObject* Template,
	bool bCopyTransientsFromClassDefaults,
	void* InstanceGraph,
	bool bAssumeTemplateIsArchetype
	);

inline UObject* (*StaticLoadObject)(
	UClass* ObjectClass,
	UObject* InOuter,
	const TCHAR* InName,
	const TCHAR* Filename,
	uint32_t LoadFlags,
	void* Sandbox,
	bool bAllowObjectReconciliation,
	void* InstancingContext
	);

inline UObject* KismetRenderingLibrary;
inline UObject* KismetStringLibrary;

inline uintptr_t gProcessEventAdd;


static class UE4
{
public:
	static auto StaticLoadObjectEasy(UClass* inClass, const wchar_t* inName, UObject* inOuter = nullptr);

	//Frees the memory for the name
	static void Free(void* buffer);

	//The same as above but for FFields.
	static std::wstring GetFirstName(FField* object);

	//Returns the very first name of the object (E.G: BP_PlayButton).
	static std::wstring GetObjectFirstName(InternalUObject* object);

	static std::wstring GetObjectName(InternalUObject* object);

	//Returns FField's type.
	static std::wstring GetFieldClassName(FField* obj);

	//Return FULL Object name including it's type.
	static std::wstring GetObjectFullName(InternalUObject* object);

	static void DumpBPs();

	static void DumpGObjects();

	//Find any entity inside the UGlobalObjects array aka. GObjects.
	template<typename T>
	static inline T FindObject(wchar_t const* name, bool ends_with = false, bool to_lower = false, int toSkip = 0)
	{
		for (auto i = 0x0; i < GObjs->NumElements; ++i)
		{
			auto object = GObjs->GetByIndex(i);
			if (object == nullptr)
			{
				continue;
			}

			auto objectFullName = GetObjectFullName(object);

			if (to_lower)
			{
				std::transform(objectFullName.begin(), objectFullName.end(), objectFullName.begin(),
					[](const unsigned char c) { return std::tolower(c); });
			}

			if (!ends_with)
			{
				if (objectFullName.starts_with(name))
				{
					if (toSkip > 0)
					{
						toSkip--;
					}
					else
					{
						return reinterpret_cast<T>(object);
					}
				}
			}
			else
			{
				if (objectFullName.ends_with(name))
				{
					return reinterpret_cast<T>(object);
				}
			}
		}
		return nullptr;
	}
};
