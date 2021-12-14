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
#include "Util.h"
#include "Field.h"

inline void* (*ProcessEvent)(void*, void*, void*);
inline int (*GetViewPoint)(void*, FMinimalViewInfo*, BYTE);
inline FString(*GetObjectNameInternal)(const UObject* Object);
inline UObject* (*SpawnActor)(UObject* UWorld, UClass* Class, FTransform const* UserTransformPtr, const FActorSpawnParameters& SpawnParameters);
inline void (*GetFullName)(FField* Obj, FString& ResultString, const UObject* StopOuter, EObjectFullNameFlags Flags);
inline void (*GetObjectFullNameInternal)(UObject* Obj, FString& ResultString, const UObject* StopOuter, EObjectFullNameFlags Flags);
inline void (*FreeInternal)(void*);
inline GObjects* GObjs;
inline struct UEngine* GEngine;

inline UObject* (*StaticConstructObject)(
	UClass* Class,
	UObject* InOuter,
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


class UE4
{
public:
	auto StaticLoadObjectEasy(UClass* inClass, const wchar_t* inName, UObject* inOuter = nullptr);

	//Frees the memory for the name
	void Free(void* buffer);

	//The same as above but for FFields.
	std::wstring GetFirstName(FField* object);

	//Returns the very first name of the object (E.G: BP_PlayButton).
	std::wstring GetObjectFirstName(UObject* object);

	std::wstring GetObjectName(UObject* object);

	//Returns FField's type.
	std::wstring GetFieldClassName(FField* obj);

	//Return FULL Object name including it's type.
	std::wstring GetObjectFullName(UObject* object);

	void DumpBPs();

	void DumpGObjects();

	//Find any entity inside the UGlobalObjects array aka. GObjects.
	template <typename T>
	static T FindObject(wchar_t const* name, bool ends_with = false, bool to_lower = false, int toSkip = 0);
};
