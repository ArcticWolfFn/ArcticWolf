#pragma once
#include "ue4.h"
#include "finder.h"

struct FGameplayEffectContextHandle
{
	char UnknownData_0[0x30]; // 0x00(0x18)
};

struct FActiveGameplayEffectHandle
{
	int Handle; // 0x00(0x04)
	bool bPassedFiltersAndWasExecuted; // 0x04(0x01)
	char UnknownData_5[0x3]; // 0x05(0x03)
};

struct FGameplayAbilitySpecDef
{
	InternalUObject* Ability;
	unsigned char Unk00[0x90];
};

enum class EGameplayEffectDurationType : uint8_t
{
	Instant, Infinite, HasDuration, EGameplayEffectDurationType_MAX
};

namespace SDK {

	inline static void BP_ApplyGameplayEffectToSelf(InternalUObject* AbilitySystemComponent, InternalUObject* GameplayEffectClass)
	{
		if (Util::IsBadReadPtr(AbilitySystemComponent)) {
			PLOGE << "AbilitySystemComponent is null";
			return;
		}

		auto BP_ApplyGameplayEffectToSelfOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/GameplayAbilities.AbilitySystemComponent"), XOR(L"BP_ApplyGameplayEffectToSelf"));

		auto BP_ApplyGameplayEffectToSelf = reinterpret_cast<UFunction*>(reinterpret_cast<uintptr_t>(AbilitySystemComponent) + BP_ApplyGameplayEffectToSelfOffset);

		if (Util::IsBadReadPtr(BP_ApplyGameplayEffectToSelf)) {
			PLOGE << "BP_ApplyGameplayEffectToSelf is null";
			return;
		}

		struct
		{
			InternalUObject* GameplayEffectClass;
			float Level;
			FGameplayEffectContextHandle EffectContext;
			FActiveGameplayEffectHandle ret;
		} Params;

		Params.EffectContext = FGameplayEffectContextHandle();
		Params.GameplayEffectClass = GameplayEffectClass;
		Params.Level = 1.0;

		ProcessEvent(AbilitySystemComponent, BP_ApplyGameplayEffectToSelf, &Params);
	}

	static InternalUObject** AbilitySystemComponent = nullptr;
	static InternalUObject* DefaultGameplayEffect = nullptr;
	static TArray<struct FGameplayAbilitySpecDef>* GrantedAbilities = nullptr;

	inline static void GrantGameplayAbility(InternalUObject* TargetPawn, InternalUObject* GameplayAbilityClass)
	{
		if (Util::IsBadReadPtr(AbilitySystemComponent))
		{
			AbilitySystemComponent = reinterpret_cast<InternalUObject**>(__int64(TargetPawn) + static_cast<__int64>(ObjectFinder::FindOffset(L"Class /Script/FortniteGame.FortPawn", L"AbilitySystemComponent")));
		}

		if (Util::IsBadReadPtr(AbilitySystemComponent)) {
			PLOGE << "AbilitySystemComponent is null";
			return;
		}

		if (Util::IsBadReadPtr(DefaultGameplayEffect))
		{
			DefaultGameplayEffect = UE4::FindObject<InternalUObject*>(L"GE_Athena_PurpleStuff_C /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.Default__GE_Athena_PurpleStuff_C");
		}

		if (!DefaultGameplayEffect)
		{
			DefaultGameplayEffect = UE4::FindObject<InternalUObject*>(L"GE_Athena_PurpleStuff_Health_C /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff_Health.Default__GE_Athena_PurpleStuff_Health_C");
		}

		if (Util::IsBadReadPtr(DefaultGameplayEffect))
		{
			PLOGE << "DefaultGameplayEffect is null";
			return;
		}

		if (Util::IsBadReadPtr(GrantedAbilities))
		{
			GrantedAbilities = reinterpret_cast<TArray<struct FGameplayAbilitySpecDef>*>(__int64(DefaultGameplayEffect) + static_cast<__int64>(ObjectFinder::FindOffset(L"Class /Script/GameplayAbilities.GameplayEffect", L"GrantedAbilities")));
		}

		if (Util::IsBadReadPtr(GrantedAbilities)) {
			PLOGE << "GrantedAbilities is null";
			return;
		}

		// overwrite current gameplay ability with the one we want to activate
		GrantedAbilities->operator[](0).Ability = GameplayAbilityClass;

		// give this gameplay effect an infinite duration
		*reinterpret_cast<EGameplayEffectDurationType*>(__int64(DefaultGameplayEffect) + static_cast<__int64>(ObjectFinder::FindOffset(L"EnumProperty /Script/GameplayAbilities.GameplayEffect", L"DurationPolicy"))) = EGameplayEffectDurationType::Infinite;

		// apply modified gameplay effect to ability system component
		static auto GameplayEffectClass = UE4::FindObject<InternalUObject*>(L"BlueprintGeneratedClass /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.GE_Athena_PurpleStuff_C");
		if (!GameplayEffectClass)
		{
			GameplayEffectClass = UE4::FindObject<InternalUObject*>(L"BlueprintGeneratedClass /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff_Health.GE_Athena_PurpleStuff_Health_C");
		}

		if (Util::IsBadReadPtr(GameplayEffectClass)) {
			PLOGE << "GameplayEffectClass is null";
			return;
		}

		BP_ApplyGameplayEffectToSelf(*AbilitySystemComponent, GameplayEffectClass);
	}
}