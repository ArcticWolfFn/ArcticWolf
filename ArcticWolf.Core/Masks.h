#pragma once

namespace Masks
{
	namespace Curl
	{
		struct Old
		{
			const char* CurlEasySetOpt = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
			const char* CurlSetOpt = "xxxxxxxxxxxxxxxxxxxxxxxxxxxx";
		};

		struct New
		{
			const char* CurlSetOpt = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
		};
	}

	struct New
	{
		const char* GEngine = "xxx????xxxxxxxxx";
		const char* FNameToString = "xxxxxx????";
		const char* ProcessEvent = "x?xxxxxxxxxx????xxxx";
		const char* GONI = "x????xxxxxxxxx";
	};

	struct Global
	{
		const char* GEngine = "xxx????xxxx????xx";
		const char* GObjects = "xxx????xxxx";
		const char* SCOI = "xxxxxxxxxxxxxxxxxxxxx????xxx????xxx????xxx";
		const char* SLOI = "xxxx?xxxx?xxxx?xxxxxxxxxxxxxxx";
		const char* ProcessEvent = "xxxxxxxxxxxxxxx????xxxx?xxx????xxx????xxxxxx????xxxxxxxx????xxxxxxxxxxx????xx";
		const char* GONI = "xxxxxxxxxxxxxxxxxxxxxxxxxxx????xxxxxxxxxxxxxxxxxx????xxxxxx????xx????x????xxxxxxxxxxxxx";
		const char* GetObjectFullName = "xxxxxxxxxx????xxx????xxxxxxx????xxxxxx";
		const char* GetFullName = "xxxx?xxxx?xxxxxxxxx?x????xx";
		const char* GetViewPoint = "xxxx?xxxx?xxxxxxxxxxxxxxxxxxx????x????xxx";
		const char* AbilityPatch = "xxx?xxxxxxxx??xx";
		const char* FreeInternal = "xxxxxxxxxxxxxxxx????xxxxx";
	};

	namespace Old
	{
		struct Global
		{
			const char* GetObjectFullName = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
		};

		struct Marshmallow
		{
			const char* GEngine = "xxx?????xxxxxxxx????xxx";

			const char* GObjects = "xxx????xxxxxxxxxx";

			const char* GetObjectFullName = "xxxxxxxxxxxxxxxxxxxxxx?????x????xxxxxx????xx????x????xxxxxxxxxxxxx";
		};
	}
}