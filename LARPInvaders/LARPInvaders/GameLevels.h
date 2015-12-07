#ifndef GAMELEVELS_H
#define GAMELEVELS_H

#include "StdReferences.h"
#include "GameConstants.h"
#include "Level.h"

class GameLevels 
{

public:
	GameLevels();

	int levelsCount;

	Level levels[MAX_LEVELS];

	void Load(std::string fileName);
};

#endif