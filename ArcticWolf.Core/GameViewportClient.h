#pragma once

#include "Finder.h"

class UGameViewportClient : GIObject
{
public:
	UGameViewportClient(ObjectFinder GameViewportClientFinder);

	void Setup() override;

	UWorld* World = nullptr;

private:
	ObjectFinder InternalFinder;
};

