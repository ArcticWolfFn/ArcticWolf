#include "pch.h"
#include "Vector.h"

FVector::FVector() : X(0), Y(0), Z(0)
{
}

FVector::FVector(float x, float y, float z) : X(x), Y(y), Z(z)
{
}

FVector FVector::operator-(FVector v)
{
	return FVector(X - v.X, Y - v.Y, Z - v.Z);
}

FVector FVector::operator+(FVector v)
{
	return FVector(X + v.X, Y + v.Y, Z + v.Z);
}

float FVector::Distance(FVector v)
{
	return (
		(X - v.X) * (X - v.X) +
		(Y - v.Y) * (Y - v.Y) +
		(Z - v.Z) * (Z - v.Z)
		);
}