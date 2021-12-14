#pragma once

#include "../Core/Character.h"
#include "../../structs.h"


namespace Game
{
	namespace Fort
	{
		class FortPlayerCharacter : public Core::Character
		{
		public:
			virtual void Setup() override;
		};
	}
}