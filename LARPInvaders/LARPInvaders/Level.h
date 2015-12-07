#ifndef LEVEL_H
#define LEVEL_H

#include "GameConstants.h"
#include "LevelPlayerInfo.h"
#include "LevelEnemyInfo.h"

class Level
{
public:
	Level();

	int ID;
	int enemyCount;
	int musicID;

	LevelPlayerInfo levelPlayerInfo;
	LevelEnemyInfo levelEnemyInfo[MAX_ENEMIES];
	
};

#endif