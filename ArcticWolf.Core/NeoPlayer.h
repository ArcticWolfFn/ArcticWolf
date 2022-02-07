#pragma once
#include "finder.h"
#include "SDK.h"
#include "Pawn.h"

struct USkeletalMeshComponent_GetAnimInstance_Params
{
	InternalUObject* ReturnValue;
};

struct UFortGadgetItemDefinition_GetWeaponItemDefinition_Params
{
	InternalUObject* ReturnValue;
};

struct UKismetSystemLibrary_ExecuteConsoleCommand_Params
{
	InternalUObject* WorldContextObject;
	FString Command;
	InternalUObject* SpecificPlayer;
};

enum class EMontagePlayReturnType : uint8_t
{
	MontageLength = 0,
	Duration = 1,
	EMontagePlayReturnType_MAX = 2
};

struct FFortAthenaLoadout
{
	FString BannerIconId;
	FString BannerColorId;
	UObject* SkyDiveContrail;
	UObject* Glider;
	UObject* Pickaxe;
	bool bIsDefaultCharacter;
	unsigned char UnknownData00[0x7];
	UObject* Character;
	TArray<UObject*> CharacterVariantChannels;
	bool bForceUpdateVariants;
	unsigned char UnknownData01[0x7];
	UObject* Hat;
	UObject* Backpack;
	UObject* LoadingScreen;
	UObject* BattleBus;
	UObject* VehicleDecoration;
	UObject* CallingCard;
	UObject* MapMarker;
	TArray<UObject*> Dances;
	UObject* VictoryPose;
	UObject* MusicPack;
	UObject* ItemWrapOverride;
	TArray<UObject*> ItemWraps;
	UObject* CharmOverride;
	TArray<UObject*> Charms;
	UObject* PetSkin;
};

struct AFortPawn_SetShield_Params
{
	float NewShieldValue;
};

struct PlayerControllerBoolsForInfiniteAmmo
{
	bool bEnableVoiceChatPTT : 1;
	bool bVoiceChatPTTTransmit : 1;
	bool bInfiniteAmmo : 1;
	bool bInfiniteMagazine : 1;
	bool bNoCoolDown : 1;
	bool bInfiniteDurability : 1;
	bool bUsePickers : 1;
	bool bPickerOpen : 1;
};

struct AFortPawn_SetMaxShield_Params
{
	float NewValue;
};

struct AFortPawn_SetMaxHealth_Params
{
	float NewHealthVal;
};

struct UAnimInstance_Montage_Play_Params
{
	InternalUObject* MontageToPlay;
	float InPlayRate;
	EMontagePlayReturnType ReturnValueType;
	float InTimeToStartMontageAt;
	bool bStopAllMontages;
	float ReturnValue;
};

struct AFortPawn_SetMovementSpeedMultiplier_Params
{
	float NewMovementSpeedVal;
};

enum class ENetRole : uint8_t
{
	ROLE_None = 0,
	ROLE_SimulatedProxy = 1,
	ROLE_AutonomousProxy = 2,
	ROLE_Authority = 3,
	ROLE_MAX = 4
};

struct USkinnedMeshComponent_SetSkeletalMesh_Params
{
	UObject* NewMesh;
	bool bReinitPose;
};

struct AFortPawn_SetHealth_Params
{
	float NewHealthVal;
};

struct AActor_K2_TeleportTo_Params
{
	FVector DestLocation;
	FRotator DestRotation;
	bool ReturnValue;
};

struct UCheatManager_Summon_Params
{
	FString ClassName;
};

struct ACharacter_IsSkydiving_Params
{
	bool ReturnValue;
};

struct AController_Possess_Params
{
	InternalUObject* InPawn;
};

struct ACharacter_IsInAircraft_Params
{
	bool ReturnValue;
};

struct AFortPlayerPawnAthena_TeleportToSkyDive_Params
{
	float HeightAboveGround;
};

enum class EMovementMode : uint8_t
{
	MOVE_None = 0,
	MOVE_Walking = 1,
	MOVE_NavWalking = 2,
	MOVE_Falling = 3,
	MOVE_Swimming = 4,
	MOVE_Flying = 5,
	MOVE_Custom = 6,
	MOVE_MAX = 7
};

struct UCharacterMovementComponent_SetMovementMode_Params
{
	TEnumAsByte<EMovementMode> NewMovementMode;
	unsigned char NewCustomMode;
};

enum class EFortCustomBodyType : uint8_t
{
	NONE = 0,
	Small = 1,
	Medium = 2,
	MediumAndSmall = 3,
	Large = 4,
	LargeAndSmall = 5,
	LargeAndMedium = 6,
	All = 7,
	Deprecated = 8,
	EFortCustomBodyType_MAX = 9
};

struct AActor_K2_GetActorLocation_Params
{
	FVector ReturnValue;
};

enum class EFortCustomGender : uint8_t
{
	Invalid = 0,
	Male = 1,
	Female = 2,
	Both = 3,
	EFortCustomGender_MAX = 4
};

struct UFortMontageItemDefinitionBase_GetAnimationHardReference_Params
{
	TEnumAsByte<EFortCustomBodyType> BodyType;
	TEnumAsByte<EFortCustomGender> Gender;
	InternalUObject* PawnContext;
	InternalUObject* ReturnValue;
};

struct ACharacter_IsJumpProvidingForce_Params
{
	bool ReturnValue;
};

struct UAnimInstance_GetCurrentActiveMontage_Params
{
	InternalUObject* ReturnValue;
};

struct ACharacter_IsParachuteOpen_Params
{
	bool ReturnValue;
};

struct UAnimInstance_Montage_Stop_Params
{
	float InBlendOutTime;
	UObject* Montage;
};

struct FGuid
{
	int A;
	int B;
	int C;
	int D;
};

struct AFortPawn_EquipWeaponDefinition_Params
{
	InternalUObject* WeaponData;
	FGuid ItemEntryGuid;
	InternalUObject* ReturnValue;
};

struct ACharacter_IsParachuteForcedOpen_Params
{
	bool ReturnValue;
};

struct UFortKismetLibrary_UpdatePlayerCustomCharacterPartsVisualization_Params
{
	InternalUObject* PlayerState;
};

class NeoPlayerClass
{
public:
	InternalUObject* Pawn;
	InternalUObject* Mesh;
	InternalUObject* AnimInstance;;
	std::wstring SkinOverride;

	// Cached Functions
	UFunction* IsInAircraftFn;
	UFunction* IsSkydivingFn;
	UFunction* IsParachuteOpenFn;
	UFunction* IsParachuteForcedOpenFn;
	UFunction* IsJumpProvidingForceFn;
	UFunction* SetVisibityFn;
	UFunction* GetAnimInstance;
	UFunction* K2_TeleportTo;
	UFunction* K2_GetActorLocation;
	UFunction* CM_Summon;
	UFunction* PossessFn;
	UFunction* TeleportToSkyDive;
	UFunction* GetCurrentActiveMontage;
	UFunction* Montage_Stop;
	UFunction* SetSkeletalMeshFn;
	UFunction* UpdatePlayerCustomCharacterPartsVisualization;
	UFunction* SetMovementModeFn;

	// Cached Objects
	UObject* Hud;
	UObject* KismetLib;

	void Setup() {
		// cache object pointers, they shouldn't change during the match (I hope)
		IsInAircraftFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerController:IsInAircraft"));
		IsSkydivingFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:IsSkydiving"));
		IsParachuteOpenFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:IsParachuteOpen"));
		IsParachuteForcedOpenFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:IsParachuteForcedOpen"));
		IsJumpProvidingForceFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Character:IsJumpProvidingForce"));
		SetVisibityFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/CommonUI.CommonActivatableWidget:ActivateWidget"));
		GetAnimInstance = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.SkeletalMeshComponent:GetAnimInstance"));
		K2_TeleportTo = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Actor:K2_TeleportTo"));
		CM_Summon = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.CheatManager:Summon"));
		PossessFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Controller:Possess"));
		TeleportToSkyDive = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawnAthena:TeleportToSkyDive"));
		GetCurrentActiveMontage = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.AnimInstance:GetCurrentActiveMontage"));
		Montage_Stop = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.AnimInstance:Montage_Stop"));
		SetSkeletalMeshFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.SkinnedMeshComponent:SetSkeletalMesh"));
		UpdatePlayerCustomCharacterPartsVisualization = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortKismetLibrary:UpdatePlayerCustomCharacterPartsVisualization"));
		K2_GetActorLocation = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Actor:K2_GetActorLocation"));
		SetMovementModeFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.CharacterMovementComponent:SetMovementMode"));

		Hud = UE4::FindObject<UObject*>(XOR(L"AthenaHUDMenu_C /Engine/Transient.FortEngine_"));
		KismetLib = UE4::FindObject<UObject*>(XOR(L"FortKismetLibrary /Script/FortniteGame.Default__FortKismetLibrary"));
	}

	// not working in season 15
	// idk what this does tbh
	void SetupAbilities() {
		if (!Util::IsBadReadPtr(Pawn)) {
			InternalUObject* jumpAbility = UE4::FindObject<InternalUObject*>(L"Class /Script/FortniteGame.FortGameplayAbility_Jump");
			if (!Util::IsBadReadPtr(jumpAbility)) {
				SDK::GrantGameplayAbility(Pawn, jumpAbility);
				PLOGD << "Granted Jump Ability";
			}
			else {
				PLOGE << "jumpAbility is null";
			}

			InternalUObject* sprintAbility = UE4::FindObject<InternalUObject*>(L"Class /Script/FortniteGame.FortGameplayAbility_Sprint");
			if (!Util::IsBadReadPtr(sprintAbility)) {
				SDK::GrantGameplayAbility(Pawn, sprintAbility);
				PLOGD << "Granted Sprint Ability";
			}
			else {
				PLOGE << "sprintAbility is null";
			}

			InternalUObject* interactAbility = UE4::FindObject<InternalUObject*>(L"BlueprintGeneratedClass /Game/Abilities/Player/Generic/Traits/DefaultPlayer/GA_DefaultPlayer_InteractUse.GA_DefaultPlayer_InteractUse_C");
			if (!Util::IsBadReadPtr(interactAbility)) {
				SDK::GrantGameplayAbility(Pawn, interactAbility);
				PLOGD << "Granted Interact Ability";
			}
			else {
				PLOGE << "interactAbility is null";
			}

			InternalUObject* searchAbility = UE4::FindObject<InternalUObject*>(L"BlueprintGeneratedClass /Game/Abilities/Player/Generic/Traits/DefaultPlayer/GA_DefaultPlayer_InteractSearch.GA_DefaultPlayer_InteractSearch_C");
			if (!Util::IsBadReadPtr(searchAbility)) {
				SDK::GrantGameplayAbility(Pawn, searchAbility);
				PLOGD << "Granted Search Ability";
			}
			else {
				PLOGE << "searchAbility is null";
			}
		}
		else {
			PLOGE << "Pawn is null";
		}
	}

	void UpdateMesh()
	{
		ObjectFinder PawnFinder = ObjectFinder::EntryPoint(uintptr_t(this->Pawn));

		ObjectFinder MeshFinder = PawnFinder.Find(XOR(L"Mesh"));
		this->Mesh = MeshFinder.GetObj();
	}

	void UpdateAnimInstance()
	{
		if (!this->Mesh || !Util::IsBadReadPtr(this->Mesh))
		{
			this->UpdateMesh();
		}

		if (Util::IsBadReadPtr(GetAnimInstance))
		{
			PLOGE << "GetAnimInstance is nullptr";
			return;
		}

		USkeletalMeshComponent_GetAnimInstance_Params GetAnimInstance_Params;

		ProcessEvent(this->Mesh, GetAnimInstance, &GetAnimInstance_Params);

		this->AnimInstance = GetAnimInstance_Params.ReturnValue;
	}

	void Authorize()
	{
		const auto LocalRole = reinterpret_cast<TEnumAsByte<ENetRole>*>(reinterpret_cast<uintptr_t>(this->Pawn) + ObjectFinder::FindOffset(
			XOR(L"Class /Script/Engine.Actor"), XOR(L"Role")));

		*LocalRole = ENetRole::ROLE_Authority;

		const auto RemoteRole = reinterpret_cast<TEnumAsByte<ENetRole>*>(reinterpret_cast<uintptr_t>(this->Pawn) + ObjectFinder::FindOffset(
			XOR(L"Class /Script/Engine.Actor"), XOR(L"RemoteRole")));

		*RemoteRole = ENetRole::ROLE_Authority;
	}

	void Respawn()
	{
		if (this->Pawn)
		{
			GGameEngine.GameViewport->World->SpawnActorEasy(UE4::FindObject<InternalUClass*>(XOR(L"BlueprintGeneratedClass /Game/Athena/PlayerPawn_Athena.PlayerPawn_Athena_C")));
			this->Pawn = ObjectFinder::FindActor(XOR(L"PlayerPawn_Athena_C"));

			if (this->Pawn)
			{
				this->Possess();
				this->ShowSkin();
				this->ShowPickaxe();
				this->UpdateAnimInstance();
			}
		}
	}

	void TeleportTo(FVector Location, FRotator Rotation)
	{
		AActor_K2_TeleportTo_Params K2_TeleportTo_Params;
		K2_TeleportTo_Params.DestLocation = Location;
		K2_TeleportTo_Params.DestRotation = Rotation;

		ProcessEvent(this->Pawn, K2_TeleportTo, &K2_TeleportTo_Params);
	}

	void Possess()
	{
		auto pawn = APawn(Pawn);
		GetGame()->LocalPlayers[0].GetPlayerController()->Possess(&pawn);

		PLOGD << "PlayerPawn was possessed";
	}

	auto StartSkydiving(float height)
	{
		AFortPlayerPawnAthena_TeleportToSkyDive_Params params;
		params.HeightAboveGround = height;

		ProcessEvent(this->Pawn, TeleportToSkyDive, &params);

		PLOGI.printf("Skydiving!, Redeploying at %fm.", height);
	}

	bool IsJumpProvidingForce()
	{
		ACharacter_IsJumpProvidingForce_Params params;

		ProcessEvent(this->Pawn, IsJumpProvidingForceFn, &params);

		return params.ReturnValue;
	}

	auto StopMontageIfEmote()
	{
		if (!this->Mesh || !this->AnimInstance || !Util::IsBadReadPtr(this->Mesh) || !Util::IsBadReadPtr(this->AnimInstance))
		{
			this->UpdateMesh();
			this->UpdateAnimInstance();
		}

		UAnimInstance_GetCurrentActiveMontage_Params GetCurrentActiveMontage_Params;

		ProcessEvent(this->AnimInstance, GetCurrentActiveMontage, &GetCurrentActiveMontage_Params);

		auto CurrentPlayingMontage = GetCurrentActiveMontage_Params.ReturnValue;

		/*if (CurrentPlayingMontage && UE4::GetObjectFirstName(CurrentPlayingMontage).starts_with(XOR(L"Emote_")))
		{
			UAnimInstance_Montage_Stop_Params Montage_Stop_Params;
			Montage_Stop_Params.InBlendOutTime = 0;
			Montage_Stop_Params.Montage = CurrentPlayingMontage;

			ProcessEvent(this->AnimInstance, Montage_Stop, &Montage_Stop_Params);
		}*/
	}

	bool IsSkydiving()
	{
		if (Util::IsBadReadPtr(IsInAircraftFn)) {
			PLOGE << "Failed to check if player is in aircraft: Function is nullptr";
			return NULL;
		}

		ACharacter_IsSkydiving_Params params;

		ProcessEvent(this->Pawn, IsSkydivingFn, &params);

		return params.ReturnValue;
	}

	bool IsParachuteOpen()
	{
		if (Util::IsBadReadPtr(IsParachuteOpenFn)) {
			PLOGE << "Failed to check if parachute is open: Function is nullptr";
			return NULL;
		}

		ACharacter_IsParachuteOpen_Params params;

		ProcessEvent(this->Pawn, IsParachuteOpenFn, &params);

		return params.ReturnValue;
	}

	bool IsParachuteForcedOpen()
	{
		if (Util::IsBadReadPtr(IsParachuteForcedOpenFn)) {
			PLOGE << "Failed to check if parachute is forced open: Function is nullptr";
			return NULL;
		}

		ACharacter_IsParachuteForcedOpen_Params params;

		ProcessEvent(this->Pawn, IsParachuteForcedOpenFn, &params);

		return params.ReturnValue;
	}

	auto SetSkeletalMesh(const wchar_t* meshname)
	{
		if (!this->Mesh || !Util::IsBadReadPtr(this->Mesh))
		{
			this->UpdateMesh();
		}

		std::wstring MeshName = meshname;

		std::wstring name = MeshName + L"." + MeshName;

		auto Mesh = UE4::FindObject<UObject*>(name.c_str(), true);

		if (Mesh)
		{
			USkinnedMeshComponent_SetSkeletalMesh_Params params;
			params.NewMesh = Mesh;
			params.bReinitPose = false;

			ProcessEvent(this->Mesh, SetSkeletalMeshFn, &params);
		}
	}

	void ShowSkin()
	{
		ObjectFinder PawnFinder = ObjectFinder::EntryPoint(uintptr_t(this->Pawn));
		ObjectFinder PlayerStateFinder = PawnFinder.Find(XOR(L"PlayerState"));

		UFortKismetLibrary_UpdatePlayerCustomCharacterPartsVisualization_Params params;
		params.PlayerState = PlayerStateFinder.GetObj();

		ProcessEvent(KismetLib, UpdatePlayerCustomCharacterPartsVisualization, &params);

		PLOGI << "Character Parts should be visible now";
	}

	auto EquipWeapon(const wchar_t* weaponname, int guid = rand())
	{
		FGuid GUID;
		GUID.A = guid;
		GUID.B = guid;
		GUID.C = guid;
		GUID.D = guid;

		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPawn:EquipWeaponDefinition"));

		std::wstring WeaponName = weaponname;

		std::wstring name = WeaponName + L"." + WeaponName;

		auto WeaponData = UE4::FindObject<InternalUObject*>(name.c_str(), true);

		if (WeaponData && !Util::IsBadReadPtr(WeaponData))
		{
			std::wstring objectName = UE4::GetObjectFullName(WeaponData);

			if (objectName.starts_with(L"FortWeapon") || objectName.starts_with(L"AthenaGadget") || objectName.starts_with(L"FortPlayset"))
			{
				if (objectName.starts_with(L"AthenaGadget"))
				{
					auto FUN_weapondef = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortGadgetItemDefinition:GetWeaponItemDefinition"));

					UFortGadgetItemDefinition_GetWeaponItemDefinition_Params prm_ReturnValue;

					ProcessEvent(WeaponData, FUN_weapondef, &prm_ReturnValue);

					if (prm_ReturnValue.ReturnValue && !Util::IsBadReadPtr(prm_ReturnValue.ReturnValue))
					{
						WeaponData = prm_ReturnValue.ReturnValue;
					}
				}

				//weapon found equip it
				AFortPawn_EquipWeaponDefinition_Params params;
				params.WeaponData = WeaponData;
				params.ItemEntryGuid = GUID;

				ProcessEvent(this->Pawn, fn, &params);
			}
		}
		else
		{
			MessageBoxA(nullptr, XOR("This item doesn't exist!"), XOR("Cranium"), MB_OK);
		}
	}

	auto Emote(InternalUObject* EmoteDef)
	{
		//We grab the mesh from the pawn then use it to get the animation instance
		if (!this->Mesh || !this->AnimInstance || !Util::IsBadReadPtr(this->Mesh) || !Util::IsBadReadPtr(this->AnimInstance))
		{
			this->UpdateMesh();
			this->UpdateAnimInstance();
		}

		if (EmoteDef && !Util::IsBadReadPtr(EmoteDef))
		{
			//Emote Def is valid, now we grab the animation montage
			auto FUNC_GetAnimationHardReference = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortMontageItemDefinitionBase:GetAnimationHardReference"));

			UFortMontageItemDefinitionBase_GetAnimationHardReference_Params GetAnimationHardReference_Params;
			GetAnimationHardReference_Params.BodyType = EFortCustomBodyType::All;
			GetAnimationHardReference_Params.Gender = EFortCustomGender::Both;
			GetAnimationHardReference_Params.PawnContext = this->Pawn;

			ProcessEvent(EmoteDef, FUNC_GetAnimationHardReference, &GetAnimationHardReference_Params);

			auto Animation = GetAnimationHardReference_Params.ReturnValue;

			//got the animation, now play :JAM:
			auto FUNC_Montage_Play = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.AnimInstance:Montage_Play"));

			UAnimInstance_Montage_Play_Params params;
			params.MontageToPlay = Animation;
			params.InPlayRate = 1;
			params.ReturnValueType = EMontagePlayReturnType::Duration;
			params.InTimeToStartMontageAt = 0;
			params.bStopAllMontages = true;

			ProcessEvent(this->AnimInstance, FUNC_Montage_Play, &params);
		}
		else
		{
			MessageBoxA(nullptr, XOR("This item doesn't exist!"), XOR("Cranium"), MB_OK);
		}
	}

	auto GetLocation() -> FVector
	{
		AActor_K2_GetActorLocation_Params params;

		ProcessEvent(this->Pawn, K2_GetActorLocation, &params);

		return params.ReturnValue;
	}

	auto SetMovementMode(TEnumAsByte<EMovementMode> NewMode, unsigned char CustomMode)
	{
		ObjectFinder PawnFinder = ObjectFinder::EntryPoint(uintptr_t(this->Pawn));

		ObjectFinder CharMovementFinder = PawnFinder.Find(XOR(L"CharacterMovement"));

		UCharacterMovementComponent_SetMovementMode_Params params;

		params.NewMovementMode = NewMode;
		params.NewCustomMode = CustomMode;

		ProcessEvent(CharMovementFinder.GetObj(), SetMovementModeFn, &params);
	}

	auto Fly(bool bIsFlying)
	{
		TEnumAsByte<EMovementMode> NewMode;

		if (!bIsFlying) NewMode = EMovementMode::MOVE_Flying;
		else NewMode = EMovementMode::MOVE_Walking;

		SetMovementMode(NewMode, 0);
	}

	auto SetPawnGravityScale(float GravityScaleInput)
	{
		ObjectFinder PawnFinder = ObjectFinder::EntryPoint(uintptr_t(this->Pawn));

		ObjectFinder CharMovementFinder = PawnFinder.Find(XOR(L"CharacterMovement"));

		auto GravityScaleOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/Engine.CharacterMovementComponent"), XOR(L"GravityScale"));

		float* GravityScale = reinterpret_cast<float*>(reinterpret_cast<uintptr_t>(CharMovementFinder.GetObj()) + GravityScaleOffset);

		*GravityScale = GravityScaleInput;
	}

	auto SetHealth(float SetHealthInput)
	{
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPawn:SetHealth"));

		AFortPawn_SetHealth_Params params;

		params.NewHealthVal = SetHealthInput;

		ProcessEvent(this->Pawn, fn, &params);
	}

	auto SetShield(float SetShieldInput)
	{
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPawn:SetShield"));

		AFortPawn_SetShield_Params params;

		params.NewShieldValue = SetShieldInput;

		ProcessEvent(this->Pawn, fn, &params);
	}

	auto SetMaxHealth(float SetMaxHealthInput)
	{
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPawn:SetMaxHealth"));

		AFortPawn_SetMaxHealth_Params params;

		params.NewHealthVal = SetMaxHealthInput;

		ProcessEvent(this->Pawn, fn, &params);
	}

	auto SetMaxShield(float SetMaxShieldInput)
	{
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPawn:SetMaxShield"));

		AFortPawn_SetMaxShield_Params params;

		params.NewValue = SetMaxShieldInput;

		ProcessEvent(this->Pawn, fn, &params);
	}

	auto SetMovementSpeed(float SetMovementSpeedInput)
	{
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPawn:SetMovementSpeed"));

		AFortPawn_SetMovementSpeedMultiplier_Params params;

		params.NewMovementSpeedVal = SetMovementSpeedInput;

		ProcessEvent(this->Pawn, fn, &params);
	}

	auto ToggleInfiniteAmmo()
	{
		auto bEnableVoiceChatPTTOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortPlayerController"), XOR(L"bEnableVoiceChatPTT"));

		// TECHNICAL EXPLINATION: (kemo) We are doing this because InfiniteAmmo bool and some other bools live in the same offset
		// the offset has 8 bits (a bitfield), bools are only one bit as it's only 0\1 so we have a struct with 8 bools (1 byte) to be able to edit that specific bool
		auto PlayerControllerBools = reinterpret_cast<PlayerControllerBoolsForInfiniteAmmo*>(reinterpret_cast<uintptr_t>(GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject) + bEnableVoiceChatPTTOffset);

		PlayerControllerBools->bInfiniteAmmo = true;
		PlayerControllerBools->bInfiniteMagazine = true;

		PLOGD << "Applied infinite ammo";
	}

	auto ExecuteConsoleCommand(const wchar_t* command)
	{
		ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
		ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
		ObjectFinder WorldFinder = GameViewPortClientFinder.Find(XOR(L"World"));

		auto KismetSysLib = UE4::FindObject<UObject*>(XOR(L"KismetSystemLibrary /Script/Engine.Default__KismetSystemLibrary"));
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.KismetSystemLibrary:ExecuteConsoleCommand"));

		UKismetSystemLibrary_ExecuteConsoleCommand_Params params;
		params.WorldContextObject = WorldFinder.GetObj();
		params.Command = command;
		params.SpecificPlayer = GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject;

		ProcessEvent(KismetSysLib, fn, &params);
		PLOGD << "Executed a console command";
	}

	auto Skydive()
	{
		this->SetMovementMode(EMovementMode::MOVE_Custom, 4);
	}

	auto ForceOpenParachute()
	{
		this->SetMovementMode(EMovementMode::MOVE_Custom, 3);
	}

	bool IsInAircraft()
	{
		if (Util::IsBadReadPtr(IsInAircraftFn)) {
			PLOGE << "Failed to check if player is in aircraft: Function is nullptr";
			return NULL;
		}

		ACharacter_IsInAircraft_Params params;

		ProcessEvent(GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject, IsInAircraftFn, &params);
		return params.ReturnValue;
	}


	void ShowPickaxe()
	{
		auto CosmeticLoadoutPCOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortPlayerController"), XOR(L"CosmeticLoadoutPC"));

		auto CosmeticLoadoutPC = reinterpret_cast<FFortAthenaLoadout*>(reinterpret_cast<uintptr_t>(GetGame()->LocalPlayers[0].GetPlayerController()->InternalObject) + CosmeticLoadoutPCOffset);

		if (!Util::IsBadReadPtr(CosmeticLoadoutPC))
		{
			auto PickaxeFinder = ObjectFinder::EntryPoint(uintptr_t(CosmeticLoadoutPC->Pickaxe));

			auto WeaponDefFinder = PickaxeFinder.Find(XOR(L"WeaponDefinition"));

			auto Weapon = UE4::GetObjectFirstName(WeaponDefFinder.GetObj());

			this->EquipWeapon(Weapon.c_str());

			PLOGI << "Equiped pickaxe";
		}
	}
};
