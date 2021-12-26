#include "pch.h"
#include "GIObject.h"
#include "ue4.h"

EmptyParams GIObject::emptyParams = EmptyParams();

GIObject::GIObject()
{
}

void GIObject::Setup()
{
}

void GIObject::ProcessNoParamsEvent(void* obj, void* fn)
{
	ProcessEvent(obj, fn, &emptyParams);
}

InternalUObject*& GIObject::toPointerReference(InternalUObject* pointer)
{
	return *&pointer;
}
