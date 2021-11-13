#pragma once
#include "structs.h"
#include "ue4.h"
#include "finder.h"

namespace SDK {

	inline static void BP_ApplyGameplayEffectToSelf(UObject* AbilitySystemComponent, UObject* GameplayEffectClass)
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
			UObject* GameplayEffectClass;
			float Level;
			FGameplayEffectContextHandle EffectContext;
			FActiveGameplayEffectHandle ret;
		} Params;

		Params.EffectContext = FGameplayEffectContextHandle();
		Params.GameplayEffectClass = GameplayEffectClass;
		Params.Level = 1.0;

		ProcessEvent(AbilitySystemComponent, BP_ApplyGameplayEffectToSelf, &Params);
	}

	inline static void GrantGameplayAbility(UObject* TargetPawn, UObject* GameplayAbilityClass)
	{
		UObject** AbilitySystemComponent = reinterpret_cast<UObject**>(__int64(TargetPawn) + static_cast<__int64>(ObjectFinder::FindOffset(L"Class /Script/FortniteGame.FortPawn", L"AbilitySystemComponent")));
		
		if (Util::IsBadReadPtr(AbilitySystemComponent)) {
			PLOGE << "AbilitySystemComponent is null";
			return;
		}

		static UObject* DefaultGameplayEffect = UE4::FindObject<UObject*>(L"GE_Athena_PurpleStuff_C /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.Default__GE_Athena_PurpleStuff_C");
		if (!DefaultGameplayEffect)
		{
			DefaultGameplayEffect = UE4::FindObject<UObject*>(L"GE_Athena_PurpleStuff_Health_C /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff_Health.Default__GE_Athena_PurpleStuff_Health_C");
		}

		if (Util::IsBadReadPtr(DefaultGameplayEffect)) {
			PLOGE << "DefaultGameplayEffect is null";
			return;
		}

		TArray<struct FGameplayAbilitySpecDef>* GrantedAbilities = reinterpret_cast<TArray<struct FGameplayAbilitySpecDef>*>(__int64(DefaultGameplayEffect) + static_cast<__int64>(ObjectFinder::FindOffset(L"Class /Script/GameplayAbilities.GameplayEffect", L"GrantedAbilities")));

		if (Util::IsBadReadPtr(DefaultGameplayEffect)) {
			PLOGE << "GrantedAbilities is null";
			return;
		}

		// overwrite current gameplay ability with the one we want to activate
		GrantedAbilities->operator[](0).Ability = GameplayAbilityClass;

		// give this gameplay effect an infinite duration
		*reinterpret_cast<EGameplayEffectDurationType*>(__int64(DefaultGameplayEffect) + static_cast<__int64>(ObjectFinder::FindOffset(L"EnumProperty /Script/GameplayAbilities.GameplayEffect", L"DurationPolicy"))) = EGameplayEffectDurationType::Infinite;

		// apply modified gameplay effect to ability system component
		static auto GameplayEffectClass = UE4::FindObject<UObject*>(L"BlueprintGeneratedClass /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.GE_Athena_PurpleStuff_C");
		if (!GameplayEffectClass)
		{
			GameplayEffectClass = UE4::FindObject<UObject*>(L"BlueprintGeneratedClass /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff_Health.GE_Athena_PurpleStuff_Health_C");
		}

		if (Util::IsBadReadPtr(GameplayEffectClass)) {
			PLOGE << "GameplayEffectClass is null";
			return;
		}

		BP_ApplyGameplayEffectToSelf(*AbilitySystemComponent, GameplayEffectClass);
	}
}