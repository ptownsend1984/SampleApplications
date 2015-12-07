#include "PhysicalObject.h"

PhysicalObject::PhysicalObject()
{
	isAlive = false;
	hitXOffsetLeft = hitXOffsetRight = hitYOffsetTop = hitYOffsetTop = 0;
	hitPoints = 0;
	lastHitTicks = 0;
}