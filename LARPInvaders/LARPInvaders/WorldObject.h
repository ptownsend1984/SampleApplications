#ifndef WORLDOBJECT_H
#define WORLDOBJECT_H

#include "SpriteType.h"
#include "StdReferences.h"

class WorldObject
{
public:
	double xWorld;
	double yWorld;
	double wWorld;
	double hWorld;

	double xVel;
	double yVel;

	double xAccel;
	double yAccel;

	double getRight() const;
	double getBottom() const;	

	double getXCenter() const;
	double getYCenter() const;

	sprite::SpriteType spriteType;

	bool isOverlapping(const WorldObject& worldObject) const;
	bool isLeftOf(const WorldObject& worldObject) const;
	bool isRightOf(const WorldObject& worldObject) const;
	bool isTopOf(const WorldObject& worldObject) const;
	bool isBottomOf(const WorldObject& worldObject) const;

	bool isAdjacent(const WorldObject& worldObject) const;

	double getAngleTo(const WorldObject& worldObject) const;

protected:
	WorldObject();
	
};

#endif