#pragma once

#include "../GIObject.h"
#include "../../structs.h"


namespace Game
{
	namespace Core
	{
		class Character : GIObject
		{
		public:
			void Jump();

			virtual void Setup() override;

		private:
			UObject* Pawn;

			static UFunction* JumpFn;
		};
	}
}