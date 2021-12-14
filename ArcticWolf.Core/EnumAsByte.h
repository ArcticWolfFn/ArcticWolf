#pragma once

template <class TEnum>
class TEnumAsByte
{
public:
	TEnumAsByte();

	TEnumAsByte(TEnum _value);

	explicit TEnumAsByte(int32_t _value);

	explicit TEnumAsByte(uint8_t _value);

	operator TEnum() const;

	TEnum GetValue() const;

private:
	uint8_t value;
};
