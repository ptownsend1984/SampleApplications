#ifndef ENEMY_H
#define ENEMY_H

#include "GameConstants.h"
#include "PhysicalObject.h"

class Enemy : public PhysicalObject
{

public:
	Enemy();

	int spawnTicks;
	int lastActionTicks;
	int lastShotTicks;
	int enemyID;
	int maxBullets;
	double bulletSpeed;
	constants::AIType aiType;

};

#endif