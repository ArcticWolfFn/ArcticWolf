#pragma once

template <class TEnum>
class TEnumAsByte
{
public:
	TEnumAsByte()
	{
	}

	TEnumAsByte(TEnum _value) : value(static_cast<uint8_t>(_value))
	{
	}

	TEnumAsByte(int32_t _value) : value(static_cast<uint8_t>(_value))
	{
	}

	TEnumAsByte(uint8_t _value) : value(_value)
	{
	}

	operator TEnum() const
	{
		return static_cast<TEnum>(value);
	}

	TEnum GetValue() const
	{
		return static_cast<TEnum>(value);
	}

private:
	uint8_t value;
};
