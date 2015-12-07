#include "WorldObject.h"

WorldObject::WorldObject()
{
	xWorld = yWorld = wWorld = hWorld = 0;
	xVel = yVel = xAccel = yAccel = 0;
	hWorld = wWorld = 0;
	spriteType = sprite::GrassDark;
}

double WorldObject::getBottom() const { return yWorld + hWorld; }
double WorldObject::getRight() const { return xWorld + wWorld; }

double WorldObject::getXCenter() const { return (xWorld + wWorld) / 2; }
double WorldObject::getYCenter() const { return (yWorld + hWorld) / 2; }

bool WorldObject::isLeftOf(const WorldObject& worldObject) const { return getRight() <= worldObject.xWorld; } 
bool WorldObject::isRightOf(const WorldObject& worldObject) const { return xWorld >= worldObject.getRight(); }
bool WorldObject::isTopOf(const WorldObject& worldObject) const { return getBottom() <= worldObject.yWorld; }
bool WorldObject::isBottomOf(const WorldObject& worldObject) const { return yWorld >= worldObject.getBottom(); }

bool WorldObject::isOverlapping(const WorldObject& worldObject) const
{
	//If this bottom is above its top...
	if(isTopOf(worldObject))
		return false;
	//If this top is under its bottom...
	if(isBottomOf(worldObject))
		return false;
	//If this left is after its right
	if(isLeftOf(worldObject))
		return false;
	//If this right is before its left...
	if(isRightOf(worldObject))
		return false;
	return true;
}

bool WorldObject::isAdjacent(const WorldObject& worldObject) const
{	
	//If this's right is before its left...
	if(getRight() < worldObject.xWorld)
		return false;
	//If this's left is after its right...
	if(xWorld > worldObject.getRight())
		return false;
	//If this's top is below its bottom...
	if(yWorld > worldObject.getBottom())
		return false;
	//If this's bottom is below its top...
	if(getBottom() < worldObject.yWorld)
		return false;

	return true;
}

double WorldObject::getAngleTo(const WorldObject& worldObject) const
{
	//Linear transformation to make this center the origin
	//then do a dot product to determine the angle to the
	//parameter world object.
	double deltaX, deltaY, cosine;
	deltaX = worldObject.getXCenter() - getXCenter();
	deltaY = worldObject.getYCenter() - getYCenter();

	//No dividing by zero
	if(deltaX == 0 && deltaY == 0)
		return 0.0;

	//The idea here is to take a dot product of the 
	//vector from this's center to the parameter's center
	//with the right-way unit vector coming out from this.
	//The formula simplifies.
	cosine = deltaX / sqrt((deltaX * deltaX) + (deltaY * deltaY));
	return acos(cosine) * 180 / M_PI;
}