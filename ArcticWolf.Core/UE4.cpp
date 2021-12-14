#include "pch.h"
#include "UE4.h"

auto UE4::StaticLoadObjectEasy(UClass* inClass, const wchar_t* inName, UObject* inOuter)
{
	return StaticLoadObject(inClass, inOuter, inName, nullptr, 0, nullptr, false, nullptr);
}

void UE4::Free(void* buffer)
{
	FreeInternal(buffer);
}

std::wstring UE4::GetFirstName(FField* object)
{
	FString s;
	GetFullName(object, s, nullptr, EObjectFullNameFlags::None);
	std::wstring objectNameW = s.ToWString();

	std::wstring token;
	while (token != objectNameW)
	{
		token = objectNameW.substr(0, objectNameW.find_first_of(L":"));
		objectNameW = objectNameW.substr(objectNameW.find_first_of(L":") + 1);
	}

	Free((void*)s.ToWString());

	return objectNameW;
}

std::wstring UE4::GetObjectFirstName(UObject* object)
{
	const FString internalName = GetObjectNameInternal(object);
	if (!internalName.ToWString()) {
		return L"";
	}

	std::wstring name(internalName.ToWString());

	Free((void*)internalName.ToWString());

	return name;
}

std::wstring UE4::GetObjectName(UObject* object)
{
	std::wstring name(L"");
	for (auto i = 0; object; object = object->Outer, ++i)
	{
		FString internalName = GetObjectNameInternal(object);
		if (!internalName.ToWString()) break;
		name = internalName.ToWString() + std::wstring(i > 0 ? L"." : L"") + name;

		Free((void*)internalName.ToWString());
	}

	return name;
}

std::wstring UE4::GetFieldClassName(FField* obj)
{
	FString s;
	GetFullName(obj, s, nullptr, EObjectFullNameFlags::None);
	const std::wstring objectName = s.ToWString();
	auto className = Util::sSplit(objectName, L" ");

	Free((void*)s.ToWString());

	return className;
}

std::wstring UE4::GetObjectFullName(UObject* object)
{
	if (Util::IsBadReadPtr(object)) {
		return L"";
	}

	FString s;
	GetObjectFullNameInternal(object, s, nullptr, EObjectFullNameFlags::None);
	std::wstring objectNameW = s.ToWString();

	Free((void*)s.ToWString());

	return objectNameW;
}

void UE4::DumpBPs()
{
	std::wofstream log("Blueprints.log");
	for (auto i = 0x0; i < GObjs->NumElements; ++i)
	{
		auto object = GObjs->GetByIndex(i);
		if (object == nullptr)
		{
			continue;
		}

		auto ClassName = GetObjectFirstName(object->Class);

		if (ClassName == XOR(L"BlueprintGeneratedClass"))
		{
			auto objectNameW = GetObjectFirstName(object);
			log << objectNameW + L"\n";
		}
	}
	log.flush();
}

void UE4::DumpGObjects()
{
	std::wofstream log("GObjects.log");

	for (auto i = 0x0; i < GObjs->NumElements; ++i)
	{
		auto object = GObjs->GetByIndex(i);
		if (object == nullptr)
		{
			continue;
		}
		std::wstring className = GetObjectName(static_cast<UObject*>(object->Class)).c_str();
		std::wstring objectName = GetObjectFullName(object).c_str();
		std::wstring item = L"\n[" + std::to_wstring(i) + L"] Object:[" + objectName + L"] Class:[" + className + L"]\n";
		log << item;
	}
	log.flush();
}

template<typename T>
T UE4::FindObject(wchar_t const* name, bool ends_with, bool to_lower, int toSkip)
{
	return T();
}
