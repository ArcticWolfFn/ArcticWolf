#pragma once
struct FRotator
{
public:
	float Pitch;
	float Yaw;
	float Roll;

	FRotator();
	FRotator(float pitch, float yaw, float roll);
};

