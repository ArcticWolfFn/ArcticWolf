#pragma once
struct FVector
{
public:
	float X;
	float Y;
	float Z;

	FVector();
	FVector(float x, float y, float z);

	FVector operator-(FVector v);
	FVector operator+(FVector v);
	float Distance(FVector v);
};

