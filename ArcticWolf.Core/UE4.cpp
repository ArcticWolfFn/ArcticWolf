#include "pch.h"
#include "UE4.h"
#include <algorithm>

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

template <class T>
struct InternalTArray
{
	friend struct InterFString;

public:

	T* Data;
	int32_t Count;
	int32_t Max;

	InternalTArray()
	{
		Data = nullptr;
		Count = Max = 0;
	};

	int Num() const
	{
		return Count;
	};

	T& operator[](int i)
	{
		return Data[i];
	};

	const T& operator[](int i) const
	{
		return Data[i];
	};

	bool IsValidIndex(int i) const
	{
		return i < Num();
	}

	int Add(UObject* NewItem)
	{
		Count = Count + 1;
		Max = Max + 1;
		Data = static_cast<UObject**>(malloc(Count * sizeof(UObject*)));
		Data[Count - 1] = NewItem;
		return Count;
	}
};


struct InternalFString : private InternalTArray<wchar_t>
{
	InternalFString()
	{
	};

	InternalFString(const wchar_t* other)
	{
		Max = Count = *other ? std::wcslen(other) + 1 : 0;

		if (Count)
		{
			Data = const_cast<wchar_t*>(other);
		}
	}

	bool IsValid() const
	{
		return Data != nullptr;
	}

	const wchar_t* ToWString() const
	{
		return Data;
	}

	std::string ToString() const
	{
		auto length = std::wcslen(Data);

		std::string str(length, '\0');

		std::use_facet<std::ctype<wchar_t>>(std::locale()).narrow(Data, Data + length, '?', &str[0]);

		return str;
	}
};

std::wstring UE4::GetObjectName(UObject* object)
{
	std::wstring name(L"");

	// ToDo: function not found
	auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Object:GetName"));

	if (Util::IsBadReadPtr(fn))
	{
		PLOGE << "Function is bad";
		return name;
	}

	for (auto i = 0; object; object = object->Outer, ++i)
	{
		struct Params {
			InternalFString ReturnValue;
		};

		auto params = Params();
		ProcessEvent(object, fn, &params);

		InternalFString internalName = params.ReturnValue;

		if (!internalName.ToWString())
			break;

		name = internalName.ToWString() + std::wstring(i > 0 ? L"." : L"") + name;
		PLOGI.printf("NamePart: %s", name.c_str());


		Free((void*)internalName.ToWString());
	}

	PLOGI.printf("Name: %s", name.c_str());

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
