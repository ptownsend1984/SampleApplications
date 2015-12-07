#ifndef PHYSICALOBJECT_H
#define PHYSICALOBJECT_H

#include "WorldObject.h"

class PhysicalObject : public WorldObject
{

public:
	PhysicalObject();

	bool isAlive;

	double hitXOffsetLeft;
	double hitXOffsetRight;
	double hitYOffsetTop;
	double hitYOffsetBottom;

	int hitPoints;
	int lastHitTicks;

};

#endif