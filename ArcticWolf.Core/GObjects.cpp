#include "pch.h"
#include "GObjects.h"

void GObjects::NumChunks(int* start, int* end) const
{
	int cStart = 0, cEnd = 0;

	if (!cEnd)
	{
		while (true)
		{
			if (ObjectArray->Objects[cStart] == nullptr)
			{
				cStart++;
			}
			else
			{
				break;
			}
		}

		cEnd = cStart;
		while (true)
		{
			if (ObjectArray->Objects[cEnd] == nullptr)
			{
				break;
			}
			cEnd++;
		}
	}

	*start = cStart;
	*end = cEnd;
}

UObject* GObjects::GetByIndex(int32_t index) const
{
	int cStart = 0, cEnd = 0;
	int chunkIndex, chunkSize = 0xFFFF, chunkPos;
	FUObjectItem* Object;

	NumChunks(&cStart, &cEnd);

	chunkIndex = index / chunkSize;
	if (chunkSize * chunkIndex != 0 &&
		chunkSize * chunkIndex == index)
	{
		chunkIndex--;
	}

	chunkPos = cStart + chunkIndex;
	if (chunkPos < cEnd)
	{
		Object = ObjectArray->Objects[chunkPos] + (index - chunkSize * chunkIndex);
		if (!Object) { return nullptr; }

		return Object->Object;
	}

	return nullptr;
}
