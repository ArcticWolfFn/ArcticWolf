#pragma once

namespace FortniteGame
{
	class UFortKismetLibrary : public UObject
	{
	public:
		// (Final|BlueprintAuthorityOnly|Native|Static|Public|BlueprintCallable)
		void SetTimeOfDay(struct UObject* WorldContextObject, float TimeOfDay);

		void Setup() override;

	private:
		UObject* InternalFortKismetLibrary = nullptr;

		static UFunction* Fn_SetTimeOfDay;
	};
}

