#pragma once

#include "Character.h"

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