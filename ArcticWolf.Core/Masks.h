#pragma once

namespace Masks
{
	namespace Curl
	{
		static struct Old
		{
			static constexpr const char* CurlEasySetOpt = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
			static constexpr const char* CurlSetOpt = "xxxxxxxxxxxxxxxxxxxxxxxxxxxx";
		};

		static struct New
		{
			static constexpr const char* CurlSetOpt = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
		};
	}

	static struct New
	{
		static constexpr const char* GEngine = "xxx????xxxxxxxxx";
		static constexpr const char* FNameToString = "xxxxxx????";
		static constexpr const char* ProcessEvent = "x?xxxxxxxxxx????xxxx";
		static constexpr const char* GONI = "x????xxxxxxxxx";
	};

	static struct Global
	{
		static constexpr const char* GEngine = "xxx????xxxx????xx";
		static constexpr const char* GObjects = "xxx????xxxx";
		static constexpr const char* SCOI = "xxxxxxxxxxxxxxxxxxxxx????xxx????xxx????xxx";
		static constexpr const char* SLOI = "xxxx?xxxx?xxxx?xxxxxxxxxxxxxxx";
		static constexpr const char* ProcessEvent = "xxxxxxxxxxxxxxx????xxxx?xxx????xxx????xxxxxx????xxxxxxxx????xxxxxxxxxxx????xx";
		static constexpr const char* GONI = "xxxxxxxxxxxxxxxxxxxxxxxxxxx????xxxxxxxxxxxxxxxxxx????xxxxxx????xx????x????xxxxxxxxxxxxx";
		static constexpr const char* GetObjectFullName = "xxxxxxxxxx????xxx????xxxxxxx????xxxxxx";
		static constexpr const char* GetFullName = "xxxx?xxxx?xxxxxxxxx?x????xx";
		static constexpr const char* GetViewPoint = "xxxx?xxxx?xxxxxxxxxxxxxxxxxxx????x????xxx";
		static constexpr const char* AbilityPatch = "xxx?xxxxxxxx??xx";
		static constexpr const char* FreeInternal = "xxxxxxxxxxxxxxxx????xxxxx";
	};

	namespace Old
	{
		static struct Global
		{
			static constexpr const char* GetObjectFullName = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
		};

		static struct Marshmallow
		{
			static constexpr const char* GEngine = "xxx?????xxxxxxxx????xxx";

			static constexpr const char* GObjects = "xxx????xxxxxxxxxx";

			static constexpr const char* GetObjectFullName = "xxxxxxxxxxxxxxxxxxxxxx?????x????xxxxxx????xx????x????xxxxxxxxxxxxx";
		};
	}
}