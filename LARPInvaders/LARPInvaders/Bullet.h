#ifndef BULLET_H
#define BULLET_H

#include "PhysicalObject.h"

class Bullet : public PhysicalObject
{

public:

	Bullet();

	int spawnTicks;
	int lastMoveTicks;
	int ownerID;

};

#endif