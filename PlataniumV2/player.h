#pragma once
#include "structs.h"
#include "finder.h"
#include "sdk.h"

class Player
{
public:
	UObject* Controller;
	UObject* Pawn;
	UObject* Mesh;
	UObject* AnimInstance;;
	std::wstring SkinOverride;

	// Cached Functions
	UFunction* JumpFn;
	UFunction* IsInAircraftFn;
	UFunction* IsSkydivingFn;
	UFunction* IsParachuteOpenFn;
	UFunction* IsParachuteForcedOpenFn;
	UFunction* IsJumpProvidingForceFn;
	UFunction* SetVisibityFn;

	// Cached Objects
	UObject* Hud;

	// Cached Offsets
	int DualBladeMenuOffset;

	void Setup() {
		// cache object pointers, they shouldn't change during the match (I hope)
		JumpFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Character:Jump"));
		IsInAircraftFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerController:IsInAircraft"));
		IsSkydivingFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:IsSkydiving"));
		IsParachuteOpenFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:IsParachuteOpen"));
		IsParachuteForcedOpenFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:IsParachuteForcedOpen"));
		IsJumpProvidingForceFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Character:IsJumpProvidingForce"));
		SetVisibityFn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/CommonUI.CommonActivatableWidget:ActivateWidget"));

		Hud = UE4::FindObject<UObject*>(XOR(L"AthenaHUDMenu_C /Engine/Transient.FortEngine_"));

		DualBladeMenuOffset = ObjectFinder::FindOffset(L"AthenaHUDMenu_C /Engine/Transient.FortEngine_", L"DualBladeMenu.BladeMenu_MainMenu");
	}

	inline void OpenMenu()
	{
		if (Util::IsBadReadPtr(Hud)) {
			PLOGE << "AthenaHUDMenu_C is nullptr";
			return;
		}

		auto DualBladeMenu = reinterpret_cast<UObject*>(reinterpret_cast<uintptr_t>(Hud) + DualBladeMenuOffset);

		if (Util::IsBadReadPtr(DualBladeMenu)) {
			PLOGE << "DualBladeMenu is nullptr";
			return;
		}

		if (Util::IsBadReadPtr(SetVisibityFn)) {
			PLOGE << "SetVisibityFn is nullptr";
			return;
		}

		// maybe use UCommonActivatableWidget::ActivateWidget() instead

		Empty_Params Params;

		ProcessEvent(DualBladeMenu, SetVisibityFn, &Params);
		PLOGV << "InGame menu should be visible now";
	}

	// not working in season 15
	void SetupAbilities() {
		if (!Util::IsBadReadPtr(Pawn)) {
			UObject* jumpAbility = UE4::FindObject<UObject*>(L"Class /Script/FortniteGame.FortGameplayAbility_Jump");
			if (!Util::IsBadReadPtr(jumpAbility)) {
				SDK::GrantGameplayAbility(Pawn, jumpAbility);
				PLOGD << "Granted Jump Ability";
			}
			else {
				PLOGE << "jumpAbility is null";
			}

			UObject* sprintAbility = UE4::FindObject<UObject*>(L"Class /Script/FortniteGame.FortGameplayAbility_Sprint");
			if (!Util::IsBadReadPtr(sprintAbility)) {
				SDK::GrantGameplayAbility(Pawn, sprintAbility);
				PLOGD << "Granted Sprint Ability";
			}
			else {
				PLOGE << "sprintAbility is null";
			}

			UObject* interactAbility = UE4::FindObject<UObject*>(L"BlueprintGeneratedClass /Game/Abilities/Player/Generic/Traits/DefaultPlayer/GA_DefaultPlayer_InteractUse.GA_DefaultPlayer_InteractUse_C");
			if (!Util::IsBadReadPtr(interactAbility)) {
				SDK::GrantGameplayAbility(Pawn, interactAbility);
				PLOGD << "Granted Interact Ability";
			}
			else {
				PLOGE << "interactAbility is null";
			}

			UObject* searchAbility = UE4::FindObject<UObject*>(L"BlueprintGeneratedClass /Game/Abilities/Player/Generic/Traits/DefaultPlayer/GA_DefaultPlayer_InteractSearch.GA_DefaultPlayer_InteractSearch_C");
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

	void UpdatePlayerController()
	{
		ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
		ObjectFinder LocalPlayer = EngineFinder.Find(XOR(L"GameInstance")).Find(XOR(L"LocalPlayers"));

		ObjectFinder PlayerControllerFinder = LocalPlayer.Find(XOR(L"PlayerController"));
		this->Controller = PlayerControllerFinder.GetObj();
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

		auto FUNC_GetAnimInstance = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.SkeletalMeshComponent:GetAnimInstance"));

		USkeletalMeshComponent_GetAnimInstance_Params GetAnimInstance_Params;

		ProcessEvent(this->Mesh, FUNC_GetAnimInstance, &GetAnimInstance_Params);

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
			this->Summon(XOR(L"PlayerPawn_Athena_C"));
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
		const auto FUNC_K2_TeleportTo = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Actor:K2_TeleportTo"));

		AActor_K2_TeleportTo_Params K2_TeleportTo_Params;
		K2_TeleportTo_Params.DestLocation = Location;
		K2_TeleportTo_Params.DestRotation = Rotation;

		ProcessEvent(this->Pawn, FUNC_K2_TeleportTo, &K2_TeleportTo_Params);
	}

	void Summon(const wchar_t* ClassToSummon)
	{
		if (!this->Controller || Util::IsBadReadPtr(this->Controller))
		{
			UpdatePlayerController();
		}

		ObjectFinder PlayerControllerFinder = ObjectFinder::EntryPoint(uintptr_t(this->Controller));

		ObjectFinder CheatManagerFinder = PlayerControllerFinder.Find(XOR(L"CheatManager"));

		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.CheatManager:Summon"));

		const FString ClassName = ClassToSummon;

		UCheatManager_Summon_Params params;
		params.ClassName = ClassName;

		ProcessEvent(CheatManagerFinder.GetObj(), fn, &params);
		printf("\n[NeoRoyale] %ls was summoned!\n", ClassToSummon);
	}

	void Possess()
	{
		if (!this->Controller || Util::IsBadReadPtr(this->Controller))
		{
			UpdatePlayerController();
		}

		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Controller:Possess"));

		AController_Possess_Params params;
		params.InPawn = this->Pawn;

		ProcessEvent(this->Controller, fn, &params);
		printf(XOR("\n[NeoRoyale] PlayerPawn was possessed!\n"));
	}

	auto StartSkydiving(float height)
	{
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawnAthena:TeleportToSkyDive"));

		AFortPlayerPawnAthena_TeleportToSkyDive_Params params;
		params.HeightAboveGround = height;

		ProcessEvent(this->Pawn, fn, &params);
		printf("\nSkydiving!, Redeploying at %fm.\n", height);
	}

	bool IsJumpProvidingForce()
	{
		if (Util::IsBadReadPtr(IsInAircraftFn)) {
			PLOGE << "Failed to check if jump is providing force: Function is nullptr";
			return NULL;
		}

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

		auto FUNC_GetCurrentActiveMontage = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.AnimInstance:GetCurrentActiveMontage"));

		UAnimInstance_GetCurrentActiveMontage_Params GetCurrentActiveMontage_Params;

		ProcessEvent(this->AnimInstance, FUNC_GetCurrentActiveMontage, &GetCurrentActiveMontage_Params);

		auto CurrentPlayingMontage = GetCurrentActiveMontage_Params.ReturnValue;

		if (CurrentPlayingMontage && UE4::GetObjectFirstName(CurrentPlayingMontage).starts_with(XOR(L"Emote_")))
		{
			auto FUNC_Montage_Stop = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.AnimInstance:Montage_Stop"));

			UAnimInstance_Montage_Stop_Params Montage_Stop_Params;
			Montage_Stop_Params.InBlendOutTime = 0;
			Montage_Stop_Params.Montage = CurrentPlayingMontage;

			ProcessEvent(this->AnimInstance, FUNC_Montage_Stop, &Montage_Stop_Params);
		}
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

	auto Jump()
	{
		if (Util::IsBadReadPtr(JumpFn)) {
			PLOGE << "Failed to jump: Jump function is nullptr";
			return;
		}

		PLOGD << "Executed jump event";
		Empty_Params params;

		ProcessEvent(this->Pawn, JumpFn, &params);
	}

	auto SetSkeletalMesh(const wchar_t* meshname)
	{
		if (!this->Mesh || !Util::IsBadReadPtr(this->Mesh))
		{
			this->UpdateMesh();
		}

		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.SkinnedMeshComponent:SetSkeletalMesh"));

		std::wstring MeshName = meshname;

		std::wstring name = MeshName + L"." + MeshName;

		auto Mesh = UE4::FindObject<UObject*>(name.c_str(), true);

		if (Mesh)
		{
			USkinnedMeshComponent_SetSkeletalMesh_Params params;
			params.NewMesh = Mesh;
			params.bReinitPose = false;

			ProcessEvent(this->Mesh, fn, &params);
		}
	}

	void ApplyOverride()
	{
		ObjectFinder EngineFinder = ObjectFinder::EntryPoint(uintptr_t(GEngine));
		ObjectFinder GameViewPortClientFinder = EngineFinder.Find(XOR(L"GameViewport"));
		ObjectFinder WorldFinder = GameViewPortClientFinder.Find(XOR(L"World"));
		ObjectFinder PawnFinder = ObjectFinder::EntryPoint(uintptr_t(this->Pawn));
		ObjectFinder PlayerStateFinder = PawnFinder.Find(XOR(L"PlayerState"));

		auto Hero = UE4::FindObject<UObject*>(XOR(L"FortHero /Engine/Transient.FortHero_"));

		/*
		 * CharacterParts Array Indexes (typeof UCustomCharacterPart)
		 * 0 - Body (e.g: CP_031_Athena_Body_Retro)
		 * 1 - Head (e.g: M_Med_HIS_Diego_Head_01)
		 * 2 - Hat (e.g: M_Med_HIS_Diego_Hat_02)
		 * 3 - Charm (e.g: M_Commando_UR_01_Grenades)
		 */

		auto CharacterParts = reinterpret_cast<TArray<UObject*>*>(reinterpret_cast<uintptr_t>(Hero) + ObjectFinder::FindOffset(
			XOR(L"Class /Script/FortniteGame.FortHero"), XOR(L"CharacterParts")));


		if (SkinOverride == L"Thanos")
		{
			CharacterParts->operator[](1) = UE4::FindObject<UObject*>(XOR(L"CustomCharacterPart /Game/Athena/Heroes/Meshes/Heads/Dev_TestAsset_Head_M_XL.Dev_TestAsset_Head_M_XL"));
			CharacterParts->operator[](0) = UE4::FindObject<UObject*>(XOR(L"CustomCharacterPart /Game/Athena/Heroes/Meshes/Bodies/Dev_TestAsset_Body_M_XL.Dev_TestAsset_Body_M_XL"));
		}
		else if (SkinOverride == L"Chituari")
		{
			CharacterParts->operator[](1) = UE4::FindObject<UObject*>(
				XOR(L"CustomCharacterPart /Game/Characters/CharacterParts/Male/Medium/Heads/CP_Athena_Head_M_AshtonMilo.CP_Athena_Head_M_AshtonMilo"));
			CharacterParts->operator[](0) = UE4::FindObject<UObject*>(XOR(L"CustomCharacterPart /Game/Athena/Heroes/Meshes/Bodies/CP_Athena_Body_M_AshtonMilo.CP_Athena_Body_M_AshtonMilo"));
		}
#ifndef PROD
		else return;
#else
		else
		{
			CharacterParts->operator[](1) = FindObject<UObject*>(XOR(L"CustomCharacterPart /Game/Athena/Heroes/Meshes/Heads/Dev_TestAsset_Head_M_XL.Dev_TestAsset_Head_M_XL"));
			CharacterParts->operator[](0) = FindObject<UObject*>(XOR(L"CustomCharacterPart /Game/Athena/Heroes/Meshes/Bodies/Dev_TestAsset_Body_M_XL.Dev_TestAsset_Body_M_XL"));
		}

#endif

		auto KismetLib = UE4::FindObject<UObject*>(XOR(L"FortKismetLibrary /Script/FortniteGame.Default__FortKismetLibrary"));
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortKismetLibrary:ApplyCharacterCosmetics"));

		UFortKismetLibrary_ApplyCharacterCosmetics_Params params;
		params.WorldContextObject = WorldFinder.GetObj();
		params.CharacterParts = *CharacterParts;
		params.PlayerState = PlayerStateFinder.GetObj();

		ProcessEvent(KismetLib, fn, &params);

		printf(XOR("\n[NeoRoyale] Character Part overrides were applied.\n"));
	}

	void ShowSkin()
	{
		ObjectFinder PawnFinder = ObjectFinder::EntryPoint(uintptr_t(this->Pawn));
		ObjectFinder PlayerStateFinder = PawnFinder.Find(XOR(L"PlayerState"));

		auto KismetLib = UE4::FindObject<UObject*>(XOR(L"FortKismetLibrary /Script/FortniteGame.Default__FortKismetLibrary"));
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortKismetLibrary:UpdatePlayerCustomCharacterPartsVisualization"));

		UFortKismetLibrary_UpdatePlayerCustomCharacterPartsVisualization_Params params;
		params.PlayerState = PlayerStateFinder.GetObj();

		ProcessEvent(KismetLib, fn, &params);
		printf(XOR("\n[NeoRoyale] Character Parts should be visible now!.\n"));
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

		auto WeaponData = UE4::FindObject<UObject*>(name.c_str(), true);

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

	auto Emote(UObject* EmoteDef)
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
		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.Actor:K2_GetActorLocation"));

		AActor_K2_GetActorLocation_Params params;

		ProcessEvent(this->Pawn, fn, &params);

		return params.ReturnValue;
	}

	auto SetMovementMode(TEnumAsByte<EMovementMode> NewMode, unsigned char CustomMode)
	{
		ObjectFinder PawnFinder = ObjectFinder::EntryPoint(uintptr_t(this->Pawn));

		ObjectFinder CharMovementFinder = PawnFinder.Find(XOR(L"CharacterMovement"));

		auto fn = UE4::FindObject<UFunction*>(XOR(L"Function /Script/Engine.CharacterMovementComponent:SetMovementMode"));

		UCharacterMovementComponent_SetMovementMode_Params params;

		params.NewMovementMode = NewMode;
		params.NewCustomMode = CustomMode;

		ProcessEvent(CharMovementFinder.GetObj(), fn, &params);
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
		if (!this->Controller || Util::IsBadReadPtr(this->Controller))
		{
			UpdatePlayerController();
		}

		auto bEnableVoiceChatPTTOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortPlayerController"), XOR(L"bEnableVoiceChatPTT"));

		// TECHNICAL EXPLINATION: (kemo) We are doing this because InfiniteAmmo bool and some other bools live in the same offset
		// the offset has 8 bits (a bitfield), bools are only one bit as it's only 0\1 so we have a struct with 8 bools (1 byte) to be able to edit that specific bool
		auto PlayerControllerBools = reinterpret_cast<PlayerControllerBoolsForInfiniteAmmo*>(reinterpret_cast<uintptr_t>(this->Controller) + bEnableVoiceChatPTTOffset);

		PlayerControllerBools->bInfiniteAmmo = true;
		PlayerControllerBools->bInfiniteMagazine = true;

		printf(XOR("\n[NeoRoyale] You should have infinite ammo now!\n"));
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
		params.SpecificPlayer = this->Controller;

		ProcessEvent(KismetSysLib, fn, &params);
		printf(XOR("\n[NeoRoyale] Executed a console command!\n"));
	}

	auto Skydive()
	{
		/*
		if (this->IsSkydiving())
		{
			auto fn = FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:EndSkydiving"));

			ProcessEvent(this->Pawn, fn, nullptr);
		}*/

		/*auto fn = FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:BeginSkydiving"));

		AFortPlayerPawn_BeginSkydiving_Params params;
		params.bFromBus = true;

		ProcessEvent(this->Pawn, fn, &params);*/

		this->SetMovementMode(EMovementMode::MOVE_Custom, 4);
	}

	auto ForceOpenParachute()
	{
		/*
		auto fn = FindObject<UFunction*>(XOR(L"Function /Script/FortniteGame.FortPlayerPawn:BP_ForceOpenParachute"));

		Empty_Params params;

		ProcessEvent(this->Pawn, fn, &params);
		*/
		this->SetMovementMode(EMovementMode::MOVE_Custom, 3);
	}

	bool IsInAircraft()
	{
		if (!this->Controller || Util::IsBadReadPtr(this->Controller))
		{
			UpdatePlayerController();
		}

		if (Util::IsBadReadPtr(IsInAircraftFn)) {
			PLOGE << "Failed to check if player is in aircraft: Function is nullptr";
			return NULL;
		}

		ACharacter_IsInAircraft_Params params;

		ProcessEvent(this->Controller, IsInAircraftFn, &params);
		return params.ReturnValue;
	}


	void ShowPickaxe()
	{
		if (!this->Controller || Util::IsBadReadPtr(this->Controller))
		{
			UpdatePlayerController();
		}

		auto CosmeticLoadoutPCOffset = ObjectFinder::FindOffset(XOR(L"Class /Script/FortniteGame.FortPlayerController"), XOR(L"CosmeticLoadoutPC"));

		auto CosmeticLoadoutPC = reinterpret_cast<FFortAthenaLoadout*>(reinterpret_cast<uintptr_t>(this->Controller) + CosmeticLoadoutPCOffset);

		if (!Util::IsBadReadPtr(CosmeticLoadoutPC))
		{
			auto PickaxeFinder = ObjectFinder::EntryPoint(uintptr_t(CosmeticLoadoutPC->Pickaxe));

			auto WeaponDefFinder = PickaxeFinder.Find(XOR(L"WeaponDefinition"));

			auto Weapon = UE4::GetObjectFirstName(WeaponDefFinder.GetObj());

			this->EquipWeapon(Weapon.c_str());

			printf(XOR("\n[NeoRoyale] Equipped the pickaxe!\n"));
		}
	}
};