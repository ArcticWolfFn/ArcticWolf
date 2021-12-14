#include "pch.h"
#include "EnumAsByte.h"

template<class TEnum>
TEnumAsByte<TEnum>::TEnumAsByte()
{
}

template<class TEnum>
TEnumAsByte<TEnum>::TEnumAsByte(TEnum _value) : value(static_cast<uint8_t>(_value))
{
}

template<class TEnum>
TEnumAsByte<TEnum>::TEnumAsByte(int32_t _value) : value(static_cast<uint8_t>(_value))
{
}

template<class TEnum>
TEnumAsByte<TEnum>::TEnumAsByte(uint8_t _value) : value(_value)
{
}

template<class TEnum>
TEnumAsByte<TEnum>::operator TEnum() const
{
	return static_cast<TEnum>(value);
}

template<class TEnum>
TEnum TEnumAsByte<TEnum>::GetValue() const
{
	return static_cast<TEnum>(value);
}

